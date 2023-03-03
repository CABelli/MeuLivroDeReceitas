using Azure.Core;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Enum;
using MeuLivroDeReceitas.Domain.Interfaces;
using MeuLivroDeReceitas.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace MeuLivroDeReceitas.Infrastructure.Repositories
{
    public class RecipeRepository : IRecipeRepository //: GenericRepository<Recipe>, IRecipeRepository
    {
        //public RecipeRepository(DbContext dbContext) : base(dbContext)
        //{
        //}

        ApplicationDbContext _recipeContext;

        public RecipeRepository(ApplicationDbContext context) //: base(context)
        {
            _recipeContext = context;
        }

        public async Task<IEnumerable<Recipe>> GetRecipies()
        {
            return await _recipeContext.Recipies.ToListAsync();
        }

        public async Task<Recipe> GetId(Guid id)
        {
            return await _recipeContext.Recipies.FindAsync(id);
        }

        public async Task<IEnumerable<Recipe>> GetRecTitle(string title)
        {
            return await _recipeContext.Recipies.Where(x => x.Title == title).ToListAsync();
        }

        public async Task<Recipe> Create(Recipe recipe)
        {
            _recipeContext.Add(recipe);
            await _recipeContext.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe> UpdateAsync(Recipe recipe)
        {

            //var recipe = await _recipeContext.Recipies.FindAsync(recipeDraftDto.Title);

            //foreach (IFormFile obj in recipeDraftDto.DataDraft)
            //{

            //    //Imagem img = new Imagem();
            //    //img.ImagemTitulo = file.FileName;

            //    //recipe.DataDraft = recipeDraftDto.DataDraft;

            //    MemoryStream ms = new MemoryStream();

            //    obj.CopyTo(ms);

            //    recipe.DataDraft = ms.ToArray();

            //    ms.Close();
            //    ms.Dispose();

            //    //_recipeContext.Recipies.Add(recipe);
            //    //_recipeContext.SaveChanges();

            //    _recipeContext.Update(recipe);

            //    await _recipeContext.SaveChangesAsync();
            //}


            _recipeContext.Update(recipe);
            await _recipeContext.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe> GetByWhere(Expression<Func<Recipe, bool>> expression)
        {
            var recipe = await _recipeContext.Recipies.FindAsync(expression);

            return recipe;
        }
    }
}
