using MeuLivroDeReceitas.Domain.Enum;
using System.ComponentModel;

namespace MeuLivroDeReceitas.Comunicacao.Dto.Response
{
    public class RecipeResponseDTO
    {
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public string? Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public bool DataDraft { get; set; }

        public Category Category { get; set; }
    }
}
