using MeuLivroDeReceitas.Domain.Account;
using Microsoft.AspNetCore.Identity;

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
        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email,
                password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManeger.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
            }

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

