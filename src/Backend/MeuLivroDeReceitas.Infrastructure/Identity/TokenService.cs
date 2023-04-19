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

        public string RecuperarEmail(string token)
        {
            var claims = ValidarToken(token);

            var claims1 = claims.Identity;

            var claims2 = claims.Claims.FirstOrDefault();

            var claims3 = claims2.Value;

            return claims3 == null ? string.Empty : claims3;

            //return claims.FindFirst(EmailAlias).Value;
        }

        public ClaimsPrincipal ValidarToken(string token)
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
            //var symmetricKey = Convert.FromBase64String(_chaveDeSeguranca);
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveDeSeguranca));
        }
    }
}
