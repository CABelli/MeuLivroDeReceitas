using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using System.Security.Claims;

namespace MeuLivroDeReceitas.Domain.Account
{
    public interface IAuthenticate
    {
        Task<UserTokenDto> Authenticate(LoginDto loginDto);

        Task<bool> RegisterUser(LoginDto loginDto);

        Task Logout();

        Task<ApplicationUserDto> RecuperarUsuario();

        Task<bool> UserChange(UserChangeDto userChangeDto);
    }
}
