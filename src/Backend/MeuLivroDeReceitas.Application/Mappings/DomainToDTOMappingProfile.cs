using AutoMapper;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Ingredient;
using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            //CreateMap<Recipe, RecipeDTO>().ReverseMap();
           // CreateMap<Ingredient, IngredientDTO>().ReverseMap();
        }
    }
}
