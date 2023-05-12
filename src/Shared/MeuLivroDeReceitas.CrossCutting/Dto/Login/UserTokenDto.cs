namespace MeuLivroDeReceitas.CrossCutting.Dto.Login
{
    public class UserTokenDto
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
