using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateService(UserManager<ApplicationUser> userManeger,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            TokenService tokenService,
            RoleManager<IdentityRole> roleManeger)
        {
            _userManager = userManeger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _roleManager = roleManeger;
        }

        public async Task<UserTokenDto> Authenticate(LoginDto loginDto)
        {
            AuthenticateValidate(loginDto);
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.Authenticate_Error_NotFound });

            return _tokenService.GenerateToken(loginDto);
        }

        public async Task<bool> RegisterUser(LoginDto loginDto)
        {
            AuthenticateValidate( loginDto);
            if (_userManager.FindByEmailAsync(loginDto.Email).Result != null)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.RegisterUser_Error_Found });

            var applicationUser = ApplicUserReady(loginDto);

            var result = await _userManager.CreateAsync(applicationUser, loginDto.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(applicationUser, "User").Wait();

                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return result.Succeeded;
            }

            throw new ErrosDeValidacaoException(new List<string>() {string.Format(Resource.RegisterUser_Error_CreateUser, result.Errors.FirstOrDefault().Description)});
        }
         
        public async Task<bool> UserChange(UserChangeDto userChangeDto)
        {
            UserChangeValidate(userChangeDto);

            var emailUsuario = ReadEmailToken();
            var applicationUser = await _userManager.FindByEmailAsync(emailUsuario);
            if (applicationUser == null) throw new ErrosDeValidacaoException(new List<string>() { Resource.UserChange_Error_UserNotFound });

            applicationUser.PhoneNumber = userChangeDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(applicationUser);
            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.UserChange_Error_UpdateUser, result.Errors.FirstOrDefault().Description) });

            return result.Succeeded;
        }

        public async Task<bool> PasswordChangeByForgot(PasswordChangeDto passwordChangeDto)
        {
            if (passwordChangeDto.NewPassword != passwordChangeDto.RepeatNewPassword)
                throw new ErrosDeValidacaoException(new List<string>() { "Senha de confirmação diferente da senha nova" });

            var appUserDto = await RetrieveUserByIdentity();

            var appUserView = await _userManager.FindByNameAsync(appUserDto.Email);

            var rolesName = await _userManager.GetRolesAsync(appUserView);
            if (rolesName.FirstOrDefault() != "Admin")
                throw new ErrosDeValidacaoException(new List<string>() { "Somente Admin pode trocar senha" });

            var appUser = await _userManager.FindByEmailAsync(passwordChangeDto.Email);
            if (appUser == null) throw new ErrosDeValidacaoException(new List<string>() { Resource.PasswordChangeByForgot_Error_UserNotFound });

            appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, passwordChangeDto.NewPassword);

            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.UserChange_Error_UpdateUser, result.Errors.FirstOrDefault().Description) });

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        
        public async Task<ApplicationUserDto> RetrieveUserByIdentity()
        {
            var emailUsuario = ReadEmailToken();
            var appUser = await _userManager.FindByEmailAsync(emailUsuario);
            return new ApplicationUserDto { 
                Email = appUser.Email, 
                UserName = appUser.UserName, 
                PhoneNumber = appUser.PhoneNumber//,
               // Id = appUser.Id 
            };
        }

        private string ReadEmailToken()
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = authorization["Bearer".Length..].Trim();
            var emailUsuario = _tokenService.RetrieveEmailByToken(token);
            return emailUsuario;
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

        private void UserChangeValidate(UserChangeDto UserChangeDto)
        {
            var validator = new UserChangeValidator();
            var resultadUserChangeValidator = validator.Validate(UserChangeDto);
            if (!resultadUserChangeValidator.IsValid)
                throw new ErrosDeValidacaoException(resultadUserChangeValidator.Errors.Select(c => c.ErrorMessage).ToList());
        }
    }
}

