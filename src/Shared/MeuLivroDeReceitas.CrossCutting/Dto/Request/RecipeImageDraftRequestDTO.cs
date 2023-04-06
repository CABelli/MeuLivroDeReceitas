using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Request
{
    public class RecipeImageDraftRequestDTO
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fileextension")]
        public string FileExtension { get; set; }

        [JsonProperty("dataDraft")]
        public byte[] DataDraft { get; set; }
    }
}
