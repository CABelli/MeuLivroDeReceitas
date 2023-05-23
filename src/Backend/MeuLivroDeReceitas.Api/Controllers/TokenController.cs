using MeuLivroDeReceitas.CrossCutting.Dto.Login;
using MeuLivroDeReceitas.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authenticate;
        private readonly IConfiguration _configuration;

        public TokenController(IAuthenticate authenticate, IConfiguration configuration)
        {
            _authenticate = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("post-login-user")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto loginDto)
            => Ok(await _authenticate.Authenticate(loginDto));

        //[Authorize(Roles = "Admin")]
        //[ApiExplorerSettings(IgnoreApi = true)]  // o endpoint fica ignorado e nao aparece
        [HttpPost("post-user-create")]
        [Authorize]
        public async Task<ActionResult> CreateUser([FromBody] UserDto userDto) 
            => Ok(await _authenticate.AddUser(userDto));
        
        [HttpPut("put-user-change")]
        public async Task<ActionResult> UserRegisterChange([FromBody] UserChangeDto userChangeDto) 
            => Ok(await _authenticate.UserChange(userChangeDto));

        [HttpPut("put-password-change-forgot")]
        public async Task<ActionResult> PasswordChange([FromBody] PasswordChangeDto passwordChangeDto)
            => Ok(await _authenticate.PasswordChangeByForgot(passwordChangeDto));

        [HttpGet]
        [Route("get-list")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> Get() => Ok(await _authenticate.GetUsers());

    }
}
