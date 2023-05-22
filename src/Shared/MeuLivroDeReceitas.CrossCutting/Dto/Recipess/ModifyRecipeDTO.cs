using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Recipess
{
    public class ModifyRecipeDTO
    {
        public string Title { get; set; }
        
        public string? PreparationMode { get; set; }

        public int PreparationTimeMinute { get; set; }

        public ECategory Category { get; set; }

        [DefaultValue(false)]
        public bool DeleteImageFile { get; set; } = false;
    }
}
