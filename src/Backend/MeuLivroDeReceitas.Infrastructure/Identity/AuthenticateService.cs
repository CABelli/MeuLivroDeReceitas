using MeuLivroDeReceitas.CrossCutting.Dto.Login;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;
using MeuLivroDeReceitas.Domain.Account;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Infrastructure.IdentityValidator;
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
            var validator = new UserValidator(EMethodUserValidator.Authenticate);
            var resultadAddUserVal = validator.Validate(new UserValidatorDto { 
                UserName = loginDto.UserName, 
                Password = loginDto.Password});
            if (!resultadAddUserVal.IsValid)
                throw new ErrosDeValidacaoException(resultadAddUserVal.Errors.Select(c => c.ErrorMessage).ToList());

            var result = await _signInManager.PasswordSignInAsync(
                loginDto.UserName, 
                loginDto.Password, 
                false, 
                lockoutOnFailure: false);
            if (!result.Succeeded) // Acesso invalido, login ou password             
                throw new ErrosDeValidacaoException(new List<string>() { Resource.Authenticate_Error_NotFound });                 

            return _tokenService.GenerateToken(loginDto);
        }

        public async Task<bool> AddUser(UserDto userDto)
        {
            var validator = new UserValidator(EMethodUserValidator.AddUser);
            var resultadAddUserVal = validator.Validate(new UserValidatorDto { 
                UserName = userDto.UserName, 
                Email = userDto.Email, 
                Password = userDto.Password, 
                PhoneNumber = userDto.PhoneNumber});
            if (!resultadAddUserVal.IsValid)
                throw new ErrosDeValidacaoException(resultadAddUserVal.Errors.Select(c => c.ErrorMessage).ToList());

            if (_userManager.FindByEmailAsync(userDto.Email).Result != null)
                throw new ErrosDeValidacaoException(new List<string>() { Resource.RegisterUser_Error_Found });

            var applicationUser = ApplicUserReady(userDto);

            var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(applicationUser, "User").Wait();

                await _signInManager.SignInAsync(applicationUser, isPersistent: false);

                return result.Succeeded;
            }

            throw new ErrosDeValidacaoException(new List<string>() {
                string.Format(Resource.RegisterUser_Error_CreateUser, result.Errors.FirstOrDefault().Description)});
        }
         
        public async Task<bool> UserChange(UserChangeDto userChangeDto)
        {
            var validator = new UserValidator(EMethodUserValidator.UserChange);
            var resultadAddUserVal = validator.Validate(new UserValidatorDto { PhoneNumber = userChangeDto.PhoneNumber });
            if (!resultadAddUserVal.IsValid)
                throw new ErrosDeValidacaoException(resultadAddUserVal.Errors.Select(c => c.ErrorMessage).ToList());

            var nameUsuario = ReadEmailToken();
            var applicationUser = await _userManager.FindByNameAsync(nameUsuario);
            if (applicationUser == null) 
                throw new ErrosDeValidacaoException(new List<string>() { Resource.UserChange_Error_UserNotFound });

            applicationUser.PhoneNumber = userChangeDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(applicationUser);
            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { 
                    string.Format(Resource.UserChange_Error_UpdateUser, result.Errors.FirstOrDefault().Description) });

            return result.Succeeded;
        }

        public async Task<bool> PasswordChangeByForgot(PasswordChangeDto passwordChangeDto)
        {
            var appUserDto = await RetrieveUserByIdentity();
            var appUserView = await _userManager.FindByNameAsync(appUserDto.UserName);
            var rolesName = await _userManager.GetRolesAsync(appUserView);

            var validator = new UserValidator(EMethodUserValidator.PasswordChangeByForgot);
            var resultadAddUserVal = validator.Validate(new UserValidatorDto { 
                Password = passwordChangeDto.NewPassword, 
                RepeatNewPassword = passwordChangeDto.RepeatNewPassword , 
                RolesName = rolesName.ToList() });
            if (!resultadAddUserVal.IsValid)
                throw new ErrosDeValidacaoException(resultadAddUserVal.Errors.Select(c => c.ErrorMessage).ToList());          

            var appUser = await _userManager.FindByEmailAsync(passwordChangeDto.Email);
            if (appUser == null) 
                throw new ErrosDeValidacaoException(new List<string>() { Resource.PasswordChangeByForgot_Error_UserNotFound });

            appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, passwordChangeDto.NewPassword);

            var result = await _userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
                throw new ErrosDeValidacaoException(new List<string>() { 
                    string.Format(Resource.UserChange_Error_UpdateUser, result.Errors.FirstOrDefault().Description) });

            return result.Succeeded;
        }
        
        public async Task<ApplicationUserDto> RetrieveUserByIdentity()
        {
            var nameUsuario = ReadEmailToken();
            var appUser = await _userManager.FindByNameAsync(nameUsuario);
            return new ApplicationUserDto { 
                Email = appUser.Email, 
                UserName = appUser.UserName, 
                PhoneNumber = appUser.PhoneNumber
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsers()
        {
            var listUser = await _userManager.GetUsersInRoleAsync("User");
            var listUserResponseDTO = new List<UserResponseDto>();
            if (listUser != null)
                listUser.OrderBy(x => x.UserName).ToList().ForEach(userList => listUserResponseDTO.Add(new UserResponseDto()
                {
                    UserName = userList.UserName,
                    Email = userList.Email,
                    PhoneNumber = userList.PhoneNumber
                }));

            return listUserResponseDTO;
        }

        #region Private

        private string ReadEmailToken()
        {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = authorization["Bearer".Length..].Trim();
            var emailUsuario = _tokenService.RetrieveEmailByToken(token);
            return emailUsuario;
        }

        private ApplicationUser ApplicUserReady(UserDto userDto)
        {
            return new ApplicationUser
            {
                UserName = userDto.UserName,     
                Email = userDto.Email, 
                NormalizedUserName = userDto.UserName?.ToUpper(),     
                NormalizedEmail = userDto.Email?.ToUpper(), 
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }

        #endregion
    }
}

