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

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        //[ApiExplorerSettings(IgnoreApi = true)]  // o endpoint fica ignorado e nao aparece
        [Authorize]
        public async Task<ActionResult> CreateUser([FromBody] LoginDto loginDto)
        {
            var result = await _authenticate.RegisterUser(loginDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authenticate.Authenticate(loginDto);
            return GenerateToken(loginDto);
        }

        private UserToken GenerateToken(LoginDto loginDto)
        {
            var claims = new[]
            {
                new Claim("email",loginDto.Email),
                new Claim("meuValor","qualquer valor"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Gerar chave privada para assinar o Token
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Gerar a assinatura digital  de 64 bytes
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            // tempo de expiração
            var expiration = DateTime.UtcNow.AddMinutes(10);

            // gerar o tojen
            JwtSecurityToken token = new JwtSecurityToken(
                // emissor
                issuer: _configuration["Jwt:Issuer"],
                // audiencia
                audience: _configuration["Jwt:Audience"],
                // claims
                claims: claims,
                // data de expiração
                expires: expiration,
                // assinatura digital
                signingCredentials: credentials
                );

            // retorna em formto json, ao retornar os dados pegar o token entrar no site https://jwt.io/  e conferir a conversão
            // e conferir com a chave secreta do appSettings  e colar no site em VERIFY SIGNATURE  e verificar a validade
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
