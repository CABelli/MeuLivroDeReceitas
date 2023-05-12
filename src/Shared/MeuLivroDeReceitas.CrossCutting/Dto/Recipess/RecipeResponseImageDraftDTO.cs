using Newtonsoft.Json;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class RecipeResponseImageDraftDTO
    {
        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("dataDraft")]
        public byte[]? DataDraft { get; set; }

        [JsonProperty("nameFile")]
        public string? NameFile { get; set; }
    }
}
