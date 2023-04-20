using MeuLivroDeReceitas.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            return await _authenticate.Authenticate(loginDto);
        }

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("UserCreate")]
        //[ApiExplorerSettings(IgnoreApi = true)]  // o endpoint fica ignorado e nao aparece
        [Authorize]
        public async Task<ActionResult> CreateUser([FromBody] LoginDto loginDto)
        {
            var result = await _authenticate.RegisterUser(loginDto);
            return Ok();
        }

        [HttpPost("UserChange")]
        public async Task<ActionResult> UserRegisterChange([FromBody] UserChangeDto userChangeDto)
        {
            var result = await _authenticate.UserChange(userChangeDto);
            return Ok();
        }
    }
}
