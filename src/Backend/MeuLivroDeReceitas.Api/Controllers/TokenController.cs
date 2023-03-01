using MeuLivroDeReceitas.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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

        [HttpPost("CreateUser")]
        [ApiExplorerSettings(IgnoreApi = true)]  // o endpoint fica ignorado e nao aparece
        [Authorize]
        public async Task<ActionResult> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _authenticate.RegisterUser(userInfo.Email, userInfo.Password);

            if (result)
            {
                return Ok($"User {userInfo.Email} login suceessfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _authenticate.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
                //return Ok($"User {userInfo.Email} login suceessfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                return BadRequest(ModelState);
            }
        }

        private UserToken GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email",userInfo.Email),
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
