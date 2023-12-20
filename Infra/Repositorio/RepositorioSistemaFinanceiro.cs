using Domain.Interfaces.ISistemaFinanceiro;
using Entities.Entidades;
using Infra.Configuracao;
using Infra.Repositorio.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorio
{
    public class RepositorioSistemaFinanceiro: RepositoryGenerics<SistemaFinanceiro>, InterfaceSistemaFinanceiro
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public RepositorioSistemaFinanceiro()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<bool> ExecuteCopiaDepesasSistemaFinanceiro()
        {
            var listaSistemaFinanceiro = new List<SistemaFinanceiro>();

            try
            {
                using(var banco = new ContextBase(_OptionsBuilder))
                {
                    listaSistemaFinanceiro = await banco.SistemaFinanceiro
                        .Where(s => s.GerarCopiaDespesa)
                        .ToListAsync();
                }

                foreach(var item in listaSistemaFinanceiro)
                {
                    using(var banco = new ContextBase(_OptionsBuilder))
                    {
                        var dataAtual = DateTime.Now;
                        var mes = dataAtual.Month;
                        var ano = dataAtual.Year;

                        var despesaJaExiste = await ( from d in banco.Despesa
                                                      join c in banco.Categoria on d.IdCategoria equals c.Id
                                                      where c.IdSistema == item.Id
                                                      && d.Mes == mes
                                                      && d.Ano == ano
                                                      select d.Id ).AnyAsync();

                        if(!despesaJaExiste)
                        {
                            var despesasSistema = await ( from d in banco.Despesa
                                                          join c in banco.Categoria on d.IdCategoria equals c.Id
                                                          where c.IdSistema == item.Id
                                                          && d.Mes == item.MesCopia
                                                          && d.Ano == item.AnoCopia
                                                          select d ).ToListAsync();
                            despesasSistema.ForEach(d =>
                            {
                                d.DataVencimento = new DateTime(ano, mes, d.DataVencimento.Day);
                                d.Mes = mes;
                                d.Ano = ano;
                                d.DataCadastro = dataAtual;
                                d.DataAlteracao = DateTime.MinValue;
                                d.DataPagamento = DateTime.MinValue;
                                d.Pago = false;
                            });

                            if(despesasSistema.Any())
                            {
                                banco.Despesa.AddRange(despesasSistema);
                                await banco.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IList<SistemaFinanceiro>> ListaSistemasUsuario(string emailUsuario)
        {
            using(var banco = new ContextBase(_OptionsBuilder))
            {
                return await
                   ( from s in banco.SistemaFinanceiro
                     join us in banco.UsuarioSistemaFinanceiro on s.Id equals us.IdSistema
                     where us.EmailUsuario.Equals(emailUsuario)
                     select s ).AsNoTracking().ToListAsync();
            }
        }
    }
}
