using MeuLivroDeReceitas.Domain.InterfacesIdentity;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class UserLogged : IUserLogged
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenService _tokenService;
        //private readonly IUsuarioReadOnlyRepositorio _repositorio;

        public UserLogged(IHttpContextAccessor httpContextAccessor, TokenService tokenService
            // IUsuarioReadOnlyRepositorio repositorio
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            // _repositorio = repositorio;
        }

        //public async Task<Usuario> RecuperarUsuario()
        public string RecuperarUsuario()
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var token = authorization["Bearer".Length..].Trim();

            //var emailUsuario = _tokenController.RecuperarEmail(token);

            var emailUsuario = _tokenService.RecuperarEmail(token);

            //var usuario = await _repositorio.RecuperarPorEmail(emailUsuario);

            return emailUsuario;
        }
    }
}
