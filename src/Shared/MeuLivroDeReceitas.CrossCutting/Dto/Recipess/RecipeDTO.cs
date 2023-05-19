using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class RecipeDTO
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public string? FileExtension { get; set; }

        public string? DataDraft { get; set; }

        public Category CategoryRecipe { get; set; }

        public string? OldFileExtension { get; set; }
    }
}
