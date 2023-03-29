using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get()
        {
            var recipies = await _recipeService.GetRecipies();
            if (recipies == null) return NotFound("Recipies not found");
            
            return Ok(recipies);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get(string title)
        {
            Resource.Culture = new System.Globalization.CultureInfo("en");
            var recipies = await _recipeService.GetRecipiesTitle(title);
            
            return Ok(recipies);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _recipeService.GetRecipeById(id));

        [HttpPost]
        [ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] RecipeDTO recipeDTO)
        {
            Resource.Culture = new System.Globalization.CultureInfo("en-US");

            await _recipeService.Add(recipeDTO);
            return Ok(Resource.Post_Return_SuccessfullyEnteredRecipe);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] RecipeStringDraftDTO recipeStringDraftDTO)
        {
            if (recipeStringDraftDTO == null) return BadRequest();            

            await _recipeService.Update(recipeStringDraftDTO);

            return Ok(recipeStringDraftDTO);
        }

        [HttpPut]
        [Route("img")]
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

        [HttpGet]
        [Route("DownLoadimage")]
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
    }
}
