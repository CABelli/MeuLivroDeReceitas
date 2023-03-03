using MeuLivroDeReceitas.Application.DTOs;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Domain.Entities;
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
        public async Task<ActionResult<IEnumerable<RecipeDTO>>> Get()
        {
            var recipies = await _recipeService.GetRecipies();
            if (recipies == null)
            {
                return NotFound("Recipies not found");
            }
            return Ok(recipies);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<RecipeDTO>>> Get(string title)
        {
            var recipies = await _recipeService.GetRecipiesTitle(title);
            if (recipies == null)
            {
                return NotFound("Recipies not found");
            }
            return Ok(recipies);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _recipeService.GetById(id));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RecipeDTO recipeDTO)
        {
            if (recipeDTO == null)
                return BadRequest("Invalid Data");

            await _recipeService.Add(recipeDTO);

            return Ok("Ok Data");
            //return new CreatedAtRouteResult("GetCaregory", new { Title = recipeDTO.Title }, recipeDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] RecipeDraftDTO recipeDTO)
        {

            if (recipeDTO == null)
            {
                return BadRequest();
            }


            //var draft  = new byte[0] ;

            //MemoryStream ms = new MemoryStream();
            //foreach (IFormFile file in Request.Form.Files)
            //{
            //    file.CopyTo(ms);
            //    draft = ms.ToArray();
            //    ms.Close();
            //    ms.Dispose();
            //}

            await _recipeService.Update(recipeDTO);

            return Ok(recipeDTO);
        }

        [HttpPut]
        [Route("img")]
        public async Task<ActionResult> PutImage([FromForm] IFormFile file, [FromBody] RecipeImageDraftDTO recipeDTO)
        {
            if (recipeDTO == null)
            {
                return BadRequest();
            }

            var draft = new byte[0];

            MemoryStream ms = new MemoryStream();
            foreach (IFormFile fil in Request.Form.Files)
            {
                fil.CopyTo(ms);
                draft = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }

            recipeDTO.DataDraft = draft;

            await _recipeService.Update(recipeDTO);

            return Ok(recipeDTO);
        }
    }
}
