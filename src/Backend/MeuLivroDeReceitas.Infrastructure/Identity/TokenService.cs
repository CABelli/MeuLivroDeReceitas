using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class TokenService
    {
        private const string EmailAlias = "email";
        private readonly string _chaveDeSeguranca;
        private readonly double _tempoDeVidaDoTokenEmMinutos;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(double tempoDeVidaDoTokenEmMinutos, string chaveDeSeguranca, string issuer, string audience)
        {
            _tempoDeVidaDoTokenEmMinutos = tempoDeVidaDoTokenEmMinutos;
            _chaveDeSeguranca = chaveDeSeguranca;
            _issuer = issuer;
            _audience = audience;
        }
        public UserTokenDto GenerateToken(LoginDto loginDto)
        {
            var claims = new[]
            {
                new Claim("email",loginDto.UserName),
                new Claim("meuValor","qualquer valor"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Gerar a assinatura digital  de 64 bytes
            var credentials = new SigningCredentials(SimetricKey(), SecurityAlgorithms.HmacSha256);

            // tempo de expiração
            var expiration = DateTime.UtcNow.AddMinutes(_tempoDeVidaDoTokenEmMinutos);

            // gerar o tojen
            JwtSecurityToken token = new JwtSecurityToken(
                // emissor
                issuer: _issuer,
                // audiencia
                audience: _audience,
                // claims
                claims: claims,
                // data de expiração
                expires: expiration,
                // assinatura digital
                signingCredentials: credentials
                );

            // retorna em formto json, ao retornar os dados pegar o token entrar no site https://jwt.io/  e conferir a conversão
            // e conferir com a chave secreta do appSettings  e colar no site em VERIFY SIGNATURE  e verificar a validade
            return new UserTokenDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public string RetrieveEmailByToken(string token)
        {
            var claims = RetrieveClaimsPrincipalByToken(token);
            var claim = claims.Claims.FirstOrDefault()?.Value;
            return claim == null ? string.Empty : claim;
        }

        private ClaimsPrincipal RetrieveClaimsPrincipalByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var parametrosValidacao = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                IssuerSigningKey = SimetricKey(),
                ClockSkew = new TimeSpan(0),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            var claims = tokenHandler.ValidateToken(token, parametrosValidacao, out _);

            return claims;
        }

        private SymmetricSecurityKey SimetricKey()
        {
            // Gerar chave privada para assinar o Token
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveDeSeguranca));
        }
    }
}
