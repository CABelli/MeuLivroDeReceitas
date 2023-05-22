using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;

namespace MeuLivroDeReceitas.CrossCutting.Dto.CategoriesDto
{
    public class CategoryListDto
    {
        public List<CategoryListDetailsDTO> CategoryItems { get; set; } = new List<CategoryListDetailsDTO>();
    }
}
