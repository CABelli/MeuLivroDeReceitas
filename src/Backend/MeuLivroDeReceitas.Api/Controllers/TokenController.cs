﻿using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
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
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto loginDto) 
            => Ok(await _authenticate.Authenticate(loginDto));
        
        //[Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost("UserCreate")]
        //[ApiExplorerSettings(IgnoreApi = true)]  // o endpoint fica ignorado e nao aparece
        [Authorize]
        public async Task<ActionResult> CreateUser([FromBody] LoginDto loginDto) 
            => Ok(await _authenticate.RegisterUser(loginDto));
        
        [HttpPut("UserChange")]
        public async Task<ActionResult> UserRegisterChange([FromBody] UserChangeDto userChangeDto) 
            => Ok(await _authenticate.UserChange(userChangeDto));

        //[Authorize(Roles = "Admin")]
        [HttpPut("PasswordChangeForgot")]
        public async Task<ActionResult> PasswordChange([FromBody] PasswordChangeDto passwordChangeDto)
            => Ok(await _authenticate.PasswordChangeByForgot(passwordChangeDto));
    }
}
