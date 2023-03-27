using Newtonsoft.Json;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Request
{
    public class RecipeStringDraftDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("fileextension")]
        public string FileExtension { get; set; }

        [JsonProperty("dataDraft")]
        public string DataDraft { get; set; }
    }
}
