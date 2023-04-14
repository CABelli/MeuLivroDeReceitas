using MeuLivroDeReceitas.CrossCutting.EnumClass;
using System.ComponentModel;

namespace MeuLivroDeReceitas.CrossCutting.Dto.Response
{
    public class RecipeResponseDTO
    {
        public Guid Id { get; set; }

        [DisplayName("Title")]
        public string? Title { get; set; }

        public string? PreparationMode { get; set; }

        public int PreparationTime { get; set; }

        public Category Category { get; set; }

        public string? NameCategoty { get; set; }

        public bool DataDraftBool { get; set; }

        public string? FileExtension { get; set; }

        public string? DataDraftCel { get; set; }
    }
}
