using Newtonsoft.Json;

namespace MeuLivroDeReceitas.Application.DTOs
{
    public class RecipeImageDraftDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("dataDraft")]
        public byte[] DataDraft { get; set; }
    }
}
