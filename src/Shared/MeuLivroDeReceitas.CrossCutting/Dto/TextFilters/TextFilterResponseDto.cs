namespace MeuLivroDeReceitas.CrossCutting.Dto.TextFilters
{
    public class TextFilterResponseDto
    {
        public List<string> Mensagens { get; set; }

        public TextFilterResponseDto(string mensagem)
        {
            Mensagens = new List<string>
            {
                mensagem
            };
        }

        public TextFilterResponseDto(List<string> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
