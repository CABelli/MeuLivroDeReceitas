using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;

namespace MeuLivroDeReceitas.Domain.Account
{
    public interface IAuthenticate
    {
        Task<UserTokenDto> Authenticate(LoginDto loginDto);

        Task<bool> AddUser(UserDto userDto);

        Task Logout();

        Task<ApplicationUserDto> RetrieveUserByIdentity();

        Task<bool> UserChange(UserChangeDto userChangeDto);

        Task<bool> PasswordChangeByForgot(PasswordChangeDto passwordChangeDto);
    }
}
