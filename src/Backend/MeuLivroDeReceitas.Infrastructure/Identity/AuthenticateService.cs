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

        public AuthenticateService(UserManager<ApplicationUser> userManeger,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            TokenService tokenService)
        {
            _userManager = userManeger;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<UserTokenDto> Authenticate(LoginDto loginDto)
        {
            var validator = new LoginValidator(loginDto.Password.Length);
            var resultadLoginValidator = validator.Validate(loginDto);
            if (!resultadLoginValidator.IsValid)
                throw new ErrosDeValidacaoException(resultadLoginValidator.Errors.Select(c => c.ErrorMessage).ToList());

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

            throw new ErrosDeValidacaoException(new List<string>() { string.Format(Resource.RegisterUser_Error_CreateUser, result.Errors.FirstOrDefault().Description)  });
        }

        public async Task<ApplicationUserDto> RecuperarUsuario()
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var token = authorization["Bearer".Length..].Trim();

            var emailUsuario = _tokenService.RetrieveEmailByToken(token);

            var user = await _userManager.FindByEmailAsync(emailUsuario);

            return new ApplicationUserDto { Email = user.Email, UserName = user.UserName, PhoneNumber = user.PhoneNumber };
        }

        public async Task<bool> UserChange(UserChangeDto userChangeDto)
        {
            ////var appUser = ReadUserRegisterBase();
            ///
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var token = authorization["Bearer".Length..].Trim();

            var emailUsuario = _tokenService.RetrieveEmailByToken(token);

            var user = await _userManager.FindByEmailAsync(emailUsuario);

            var result = await _userManager.ChangePhoneNumberAsync(user == null ? new ApplicationUser() : user, userChangeDto.PhoneNumber, ReadToken());

            //appUser.PhoneNumber = userChangeDto.PhoneNumber;
            //var result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded)
            {
               // await _signInManager.SignInAsync(appUser, isPersistent: false);
                return result.Succeeded;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        private string ReadToken()
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = authorization["Bearer".Length..].Trim();
            return token;
        }

        public async Task<ApplicationUser>  ReadUserRegisterBase()
        {
            var emailUsuario = _tokenService.RetrieveEmailByToken(ReadToken());
            var AppUser = await _userManager.FindByEmailAsync(emailUsuario);
            return AppUser == null ? new ApplicationUser() : AppUser;
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
    }
}

