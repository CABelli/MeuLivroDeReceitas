using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        [Route("get-list")]
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get() => Ok(await _recipeService.GetRecipies());

        [HttpGet]
        [Route("get-title")]
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get(string title) => Ok(await _recipeService.GetRecipiesTitle(title));
        
        [HttpGet]
        [Route("get-id")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _recipeService.GetRecipeById(id));

        [HttpGet]
        [Route("get-downloadimage")]
        public async Task<ActionResult> DownLoadimage(string title)
        {
            var listRecImageDraftDTO = await _recipeService.GetRecipiesDownLoad(title);
            return File(listRecImageDraftDTO.FirstOrDefault().DataDraft,"image/png", listRecImageDraftDTO.FirstOrDefault().NameFile);
        }

        [HttpPost]
        [Route("post-add")]
        //[ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult> IncludeRecipe([FromBody] RecipeDTO recipeDTO)
        {
            await _recipeService.AddRecipe(recipeDTO);
            return CreatedAtAction(nameof(IncludeRecipe), Resource.IncludeRecipe_Return_Successfully);
        }

        [HttpPut]
        [Route("put-binaryfilestring")]
        public async Task<ActionResult> PutString([FromBody] RecipeDTO recipeDTO)
        {
            await _recipeService.UpdateRecipeDraftString(recipeDTO);
            return Ok(recipeDTO);
        }

        [HttpPut]
        [Route("put-binaryfileimage")]
        public async Task<ActionResult> PutImage([FromForm] ICollection<IFormFile> binaryfiles, string title, string fileExtension)
        {
            var fileName = await _recipeService.UpdateRecipeDraftImage(binaryfiles, title, fileExtension);
            return Ok(fileName);
        }

        [HttpDelete]
        [Route("delete-recipe")]
        public async Task<ActionResult> Delete(string title)
        {
            await _recipeService.DeleteRecipeByTitle(title);
            return Ok();
        }
    }
}
