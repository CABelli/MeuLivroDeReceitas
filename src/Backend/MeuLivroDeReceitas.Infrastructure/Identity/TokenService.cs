using MeuLivroDeReceitas.CrossCutting.Dto.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class TokenService
    {
        private readonly string _secretKey;
        private readonly double _tokenLifetimeInMinutes;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(double tokenLifetimeInMinutes, 
            string secretKey, 
            string issuer, 
            string audience)
        {
            _tokenLifetimeInMinutes = tokenLifetimeInMinutes;
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public UserTokenDto GenerateToken(LoginDto loginDto)
        {
            var claims = new[]
            {
                new Claim("userName",loginDto.UserName),
                new Claim("meuValor","any value"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Gerar a assinatura digital  de 64 bytes
            var credentials = new SigningCredentials(SimetricKey(), SecurityAlgorithms.HmacSha256);

            // tempo de expiração
            var expiration = DateTime.UtcNow.AddMinutes(_tokenLifetimeInMinutes);

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
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }
    }
}
