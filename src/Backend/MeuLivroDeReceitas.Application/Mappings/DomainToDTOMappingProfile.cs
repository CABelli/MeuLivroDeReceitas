using AutoMapper;
using MeuLivroDeReceitas.Application.DTOs;
using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Recipe, RecipeDTO>().ReverseMap();
            CreateMap<Ingredient, IngredientDTO>().ReverseMap();
        }
    }
}
