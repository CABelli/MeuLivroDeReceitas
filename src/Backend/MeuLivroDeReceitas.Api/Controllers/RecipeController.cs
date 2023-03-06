using MeuLivroDeReceitas.Application.DTOs;
using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.Comunicacao.Dto.Request;
using MeuLivroDeReceitas.Comunicacao.Dto.Response;
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
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get()
        {
            var recipies = await _recipeService.GetRecipies();
            if (recipies == null)
            {
                return NotFound("Recipies not found");
            }
            return Ok(recipies);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<RecipeResponseDTO>>> Get(string title)
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
        public async Task<ActionResult> Post([FromBody] Comunicacao.Dto.Request.RecipeDTO recipeDTO)
        {
            await _recipeService.Add(recipeDTO);

            return Ok("Ok Data");
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] RecipeStringDraftDTO recipeStringDraftDTO)
        {
            if (recipeStringDraftDTO == null)
            {
                return BadRequest();
            }

            await _recipeService.Update(recipeStringDraftDTO);

            return Ok(recipeStringDraftDTO);
        }

        [HttpPut]
        [Route("img")]
        public async Task<ActionResult> PutImage([FromForm] ICollection<IFormFile> files, string title)
        //([FromForm] IFormFile file, [FromBody] RecipeImageDraftRequestDTO recipeImageDraftRequestDTO)
        {
            if (title == null || files == null) return BadRequest();

            var fileDrfat = new byte[0];
            List<byte[]> lista = new();

            foreach (IFormFile fil in files)
            {
                if (fil.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fil.CopyTo(memoryStream);
                        fileDrfat = memoryStream.ToArray();
                        lista.Add(memoryStream.ToArray());
                    }
                }
                else
                    return BadRequest();
            }

            var recipeImageDraftDTO = new RecipeImageDraftRequestDTO
            {
                Title = title,
                DataDraft = fileDrfat
            };

            await _recipeService.Update(recipeImageDraftDTO);

            //return Ok("ok " + name + File(fileDrfat, contentType, "Imagem01.png") );

            var x = files.FirstOrDefault().ContentType;

            var nomeArq = DateTime.Now.ToString("HH:mm:ss") + Path.GetFileName(files.FirstOrDefault().FileName);

            return File(lista[0], files.FirstOrDefault().ContentType, nomeArq);
        }

        [HttpGet]
        [Route("DownLoadimage")]
        public async Task<ActionResult> DownLoadimage(string title)
        {
            var listRecipeImageDraftDTO = await _recipeService.GetRecipiesDownLoad(title);

            if (listRecipeImageDraftDTO == null || listRecipeImageDraftDTO.Count() == 0) { return BadRequest(); }

            return File(listRecipeImageDraftDTO.FirstOrDefault().ListDataDraft[0],
                "image/png", 
                listRecipeImageDraftDTO.FirstOrDefault().NamyFile );
        }
    }
}
