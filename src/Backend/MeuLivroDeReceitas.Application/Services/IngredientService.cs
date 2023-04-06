using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;

namespace MeuLivroDeReceitas.Application.Services
{
    public class IngredientService : IIngredientService
    {
        public IngredientService() { }

        public async Task<IEnumerable<IngredientDTO>> GetRecipies()
        {
            return new List<IngredientDTO>();
        }
    }
}
