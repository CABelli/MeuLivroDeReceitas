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
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get()
        {
            var recipies = await _recipeService.GetRecipies();            
            return Ok(recipies);
        }

        [HttpGet]
        [Route("get-title")]
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get(string title)
        {
            var recipies = await _recipeService.GetRecipiesTitle(title);            
            return Ok(recipies);
        }

        [HttpGet]
        [Route("get-id")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _recipeService.GetRecipeById(id));

        [HttpGet]
        [Route("get-downloadimage")]
        public async Task<ActionResult> DownLoadimage(string title)
        {
            var listRecipeImageDraftDTO = await _recipeService.GetRecipiesDownLoad(title);

            //if (listRecipeImageDraftDTO.Count() == 0)
            return Ok("...  Recipe not found or Title not found  ...");
            //else
            //return File(listRecipeImageDraftDTO.FirstOrDefault().DataDraft,
            //    "image/png",
            //    listRecipeImageDraftDTO.FirstOrDefault().NameFile);
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
        [Route("put-draftstring")]
        public async Task<ActionResult> PutString([FromBody] RecipeDTO recipeDTO)
        {       
            await _recipeService.Update(recipeDTO);
            return Ok(recipeDTO);
        }

        [HttpPut]
        [Route("put-draftimage")]
        public async Task<ActionResult> PutImage([FromForm] ICollection<IFormFile> files, string title, string fileExtension)
        {
            if (title == null || fileExtension == null || files == null) return BadRequest();

            var recipeImageDraftDTO = new RecipeImageDraftRequestDTO
            {
                Title = title,
                FileExtension = fileExtension 
            };

            await _recipeService.Update(files, recipeImageDraftDTO);

            var nomeArq = DateTime.Now.ToString("HH:mm:ss") + Path.GetFileName(files.FirstOrDefault().FileName);

            return Ok(nomeArq);
        }
    }
}
