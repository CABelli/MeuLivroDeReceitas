using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Application.Services;
using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
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
            return CreatedAtAction(nameof(IncludeIngredient), "Inclusão com Sucesso");

            //return CreatedAtAction(nameof(IncludeRecipe), Resource.IncludeRecipe_Return_Successfully);

        }

        [HttpGet]
        [Route("get-list")]
        public async Task<ActionResult<IngredientListDTO>> Get(string title) => Ok(await _ingredientService.GetIngredients(title));

    }
}
