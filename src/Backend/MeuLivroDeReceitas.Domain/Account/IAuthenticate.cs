using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using System.Security.Claims;

namespace MeuLivroDeReceitas.Domain.Account
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(LoginDto loginDto);

        Task<bool> RegisterUser(LoginDto loginDto);

        Task Logout();

        Task<ApplicationUserDto> RecuperarUsuario();

        //Task<ApplicationUserDto> ReadUser(string email);

        //Task<ApplicationUserDto> ReadUser(ClaimsPrincipal claims);
    }
}
