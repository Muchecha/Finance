﻿using Domain.Interfaces.IUsuarioSistemaFinanceiro;
using Entities.Entidades;
using Infra.Repositorio.Generics;

namespace Infra.Repositorio
{
    public class RepositorioUsuarioSistemaFinanceiro: RepositoryGenerics<UsuarioSistemaFinanceiro>, InterfaceUsuarioSistemaFinanceiro
    {
        public Task<IList<UsuarioSistemaFinanceiro>> ListarUsuariosSistema(int IdSistema)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioSistemaFinanceiro> ObterUsuarioPorEmail(string emailUsuario)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUsuarios(List<UsuarioSistemaFinanceiro> usuarios)
        {
            throw new NotImplementedException();
        }
    }
}