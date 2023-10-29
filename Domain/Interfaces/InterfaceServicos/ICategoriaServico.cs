using Entities.Entidades;

namespace Domain.Interfaces.InterfaceServicos
{
    public interface ICategoriaServico
    {
        Task AdicionarCategoria(Categoria catagoria);
        Task AtualizarCategoria(Categoria catagoria);
    }
}
