﻿using MeuLivroDeReceitas.Application.Interfaces;
using MeuLivroDeReceitas.CrossCutting.Dto.CategoriesDto;
using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<RecipeResponseDTO>> Get(string title) => Ok(await _recipeService.GetRecipiesTitle(title));

        [HttpGet]
        [Route("get-id")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _recipeService.GetRecipeById(id));

        [HttpGet]
        [Route("get-downloadimage")]
        public async Task<ActionResult> DownLoadimage(string title)
        {
            var listRecImageDraftDTO = await _recipeService.GetRecipiesDownLoad(title);
            return File(listRecImageDraftDTO.DataDraft, "image/png", listRecImageDraftDTO.NameFile);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get-categories")]
        public async Task<ActionResult<CategoryListDto>> ListCategories()
        {            
            var categoryList = new CategoryListDto() { };
            Enum.GetValues(typeof(ECategory)).Cast<ECategory>().ToList()
                .ForEach(categories => categoryList.CategoryItems.Add(new CategoryListDetailsDTO()
                {
                    IdCategory = (int)categories,
                    NameCategory = Enum.GetName(typeof(ECategory), categories),
                    DescriptionCategory = categories.GetLocalizedDescription()
                }));

            return categoryList;
        }

        [HttpPost]
        [Route("post-add")]
        //[ProducesResponseType(typeof(RecipeResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult> IncludeRecipe([FromBody] AddRecipeDTO addRecipeDTO)
        {
            await _recipeService.AddRecipe(addRecipeDTO);
            return CreatedAtAction(nameof(IncludeRecipe), Resource.IncludeRecipe_Return_Successfully);
        }

        [HttpPut]
        [Route("put-update")]
        public async Task<ActionResult> AjustRecipe([FromBody] ModifyRecipeDTO modifyRecipeDTO)
        {
            await _recipeService.UpdateRecipe(modifyRecipeDTO);
            return Ok(modifyRecipeDTO);
        }

        [HttpPut]
        [Route("put-binaryfileimage")]
        public async Task<ActionResult> PutImage([FromForm] ICollection<IFormFile> binaryfiles, string title)
        {
            var fileName = await _recipeService.UpdateRecipeDraftImage(binaryfiles, title);
            return Ok(fileName);
        }

        [HttpDelete]
        [Route("delete-recipe")]
        public async Task DeleteId(string title) => await _recipeService.DeleteRecipeByTitle(title);

    }
}
