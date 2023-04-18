using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;

namespace MeuLivroDeReceitas.Domain.Account
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(LoginDto loginDto);
        Task<bool> RegisterUser(LoginDto loginDto);
        Task Logout();
    }
}
