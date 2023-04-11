using Newtonsoft.Json;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Response
{
    public class RecipeImageDraftDTO
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("dataDraft")]
        public byte[]? DataDraft { get; set; }

        [JsonProperty("nameFile")]
        public string? NameFile { get; set; }
    }
}
