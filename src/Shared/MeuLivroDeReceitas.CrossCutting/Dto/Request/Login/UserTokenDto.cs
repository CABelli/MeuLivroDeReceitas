namespace MeuLivroDeReceitas.CrossCutting.Dto.Request.Login
{
    public class UserTokenDto
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
