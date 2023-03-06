using Newtonsoft.Json;

namespace MeuLivroDeReceitas.Comunicacao.Dto.Request
{
    public class RecipeImageDraftRequestDTO
    {
        [JsonProperty("id")]
        public string Title { get; set; }

        [JsonProperty("dataDraft")]
        public byte[] DataDraft { get; set; }
    }
}
