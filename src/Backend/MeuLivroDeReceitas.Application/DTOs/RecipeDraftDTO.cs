using System;
using Newtonsoft.Json;

namespace MeuLivroDeReceitas.Application.DTOs
{
    public class RecipeDraftDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("dataDraft")]
        public string DataDraft { get; set; }
    }
}
