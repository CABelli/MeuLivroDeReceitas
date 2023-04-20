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

        public TokenService(double tempoDeVidaDoTokenEmMinutos, string chaveDeSeguranca)
        {
            _tempoDeVidaDoTokenEmMinutos = tempoDeVidaDoTokenEmMinutos;
            _chaveDeSeguranca = chaveDeSeguranca;
        }

        public string RetrieveEmailByToken(string token)
        {
            var claims = RetrieveClaimsPrincipalByToken(token);
            var claim = claims.Claims.FirstOrDefault()?.Value;
            return claim == null ? string.Empty : claim;
        }
        
        public ClaimsPrincipal RetrieveClaimsPrincipalByToken(string token)
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
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveDeSeguranca));
        }
    }
}
