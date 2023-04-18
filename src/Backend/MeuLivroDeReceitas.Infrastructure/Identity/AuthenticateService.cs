using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Identity;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManeger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateService(UserManager<ApplicationUser> userManeger,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManeger = userManeger;
            _signInManager = signInManager;
        }

        public async Task<bool> Authenticate(LoginDto loginDto)
        {
            var validator = new LoginValidator(loginDto.Password.Length);
            var resultadLoginValidator = validator.Validate(loginDto);
            if (!resultadLoginValidator.IsValid)
                throw new ErrosDeValidacaoException(resultadLoginValidator.Errors.Select(c => c.ErrorMessage).ToList());

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.Authenticate_Error_NotFound });

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(LoginDto loginDto)
        {
            AuthenticateValidate( loginDto);

            if (_userManeger.FindByEmailAsync(loginDto.Email).Result != null)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.RegisterUser_Error_Found });

            var applicationUser = ApplicUserReady(loginDto);

            var result = await _userManeger.CreateAsync(applicationUser, loginDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return result.Succeeded;
            }

            throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.RegisterUser_Error_CreateUser, result.Errors.FirstOrDefault().Description)  });
        }

        private ApplicationUser ApplicUserReady(LoginDto loginDto)
        {
            return new ApplicationUser
            {
                UserName = loginDto.Email,     
                Email = loginDto.Email, 
                NormalizedUserName = loginDto.Email.ToUpper(),     
                NormalizedEmail = loginDto.Email.ToUpper(), 
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }

        private void AuthenticateValidate(LoginDto loginDto)
        {
            var validator = new LoginValidator(loginDto.Password.Length);
            var resultadLoginValidator = validator.Validate(loginDto);
            if (!resultadLoginValidator.IsValid)
                throw new ErrosDeValidacaoException(resultadLoginValidator.Errors.Select(c => c.ErrorMessage).ToList());
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

