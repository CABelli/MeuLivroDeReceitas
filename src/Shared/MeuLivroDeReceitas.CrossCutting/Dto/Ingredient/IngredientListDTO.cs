namespace MeuLivroDeReceitas.CrossCutting.Dto.Ingredient
{
    public class IngredientListDTO
    {
        public string Title { get; set; }
        public List<IngredientListDetailsDTO> RecipeItems { get; set; } = new List<IngredientListDetailsDTO>();
    }
}
