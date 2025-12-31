using AutoMapper;
using Recipes.API.Models;
using Recipes.Core.Entities;

namespace Recipes.Mappings;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        // Origen -> Destino
        CreateMap<Recipe, RecipeReadDto>();
        CreateMap<Ingredient, IngredientDto>();

        CreateMap<RecipeCreateDto, Recipe>();
        CreateMap<IngredientCreateDto, Ingredient>();
        CreateMap<RecipeUpdateDto, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients)); ;
    }
}