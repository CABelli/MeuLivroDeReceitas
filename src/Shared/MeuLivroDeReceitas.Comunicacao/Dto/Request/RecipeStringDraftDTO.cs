using Newtonsoft.Json;

namespace MeuLivroDeReceitas.Comunicacao.Dto.Request
{
    public class RecipeStringDraftDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("dataDraft")]
        public string DataDraft { get; set; }
    }
}
