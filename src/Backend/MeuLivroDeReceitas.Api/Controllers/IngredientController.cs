using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpPost]
        [Route("post-add")]
        public async Task<ActionResult> IncludeIngredient([FromBody] IngredientAddDto ingredientAddDto)
        {
            await _ingredientService.AddIngredient(ingredientAddDto);
            return CreatedAtAction(nameof(IncludeIngredient), Resource.IncludeIngredient_Return_Successfully);
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<ActionResult<IngredientListDTO>> Get(string title) => Ok(await _ingredientService.GetIngredients(title));
    }
}
