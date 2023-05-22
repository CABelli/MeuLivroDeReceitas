using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class RecipeResponseDTO
    {
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public string? Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTimeMinute { get; set; }

        public ECategory Category { get; set; }

        public string? NameCategory { get; set; }

        public string? FileExtension { get; set; }
    }
}
