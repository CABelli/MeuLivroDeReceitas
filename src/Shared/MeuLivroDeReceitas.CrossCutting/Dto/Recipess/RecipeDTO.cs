using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class RecipeDTO
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTimeMinute { get; set; }

        public ECategory Category { get; set; }

        public string? OldFileExtension { get; set; }

        public bool DeleteImageFile { get; set; }
    }
}
