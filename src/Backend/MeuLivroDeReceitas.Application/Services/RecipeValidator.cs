using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeValidator : AbstractValidator<RecipeDTO>
    {
        private int MinimumNumberOfCharactersInTitle = 5;
        private int MaximumNumberOfCharactersInTitle = 20;
        private int MinimumPreparationTimeMinutes = 3;
        private int MaximumPreparationTimeMinutes = 300;

        public RecipeValidator(EMethodRecipeValidator method)
        {
            switch (method)
            {
                case EMethodRecipeValidator.AddRecipe: ValidatorAddRecipe(); return;
                case EMethodRecipeValidator.ModifyRecipe: ValidatorModifyRecipe(); return;
            };
        }

        public void ValidatorAddRecipe()
        {
            ValidatorTitle();
            ValidatorPreparationMode();
            ValidatorPreparationTime();
            ValidatorCategory();
        }

        public void ValidatorModifyRecipe()
        {
            ValidatorTitle();
            ValidatorPreparationMode();
            ValidatorPreparationTime();
            ValidatorCategory();
            ValidatorDeleteImageFile();
        }

        public void ValidatorTitle()
        {
            RuleFor(c => c.Title).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledTitle, nameof(ValidatorTitle)));

            RuleFor(RecTile => RecTile.Title).Length(MinimumNumberOfCharactersInTitle, MaximumNumberOfCharactersInTitle)
                .WithMessage(RecTile => string.Format(Resource.RecipeValidator_Error_CharactersTitle,
                RecTile.Title.Length,
                MinimumNumberOfCharactersInTitle,
                MaximumNumberOfCharactersInTitle,
                nameof(ValidatorTitle)));
        }

        public void ValidatorPreparationMode()
        {
            RuleFor(c => c.PreparationMode).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationMode, nameof(ValidatorPreparationMode)));
        }

        public void ValidatorPreparationTime()
        {
            RuleFor(c => c.PreparationTimeMinute).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationTime, nameof(ValidatorPreparationTime)));

            RuleFor(RecTile => RecTile.PreparationTimeMinute).InclusiveBetween(MinimumPreparationTimeMinutes, MaximumPreparationTimeMinutes)
                .WithMessage(RecTile => string.Format(
                    $"Tempo de preparo deve ser entre {MinimumPreparationTimeMinutes} e {MaximumPreparationTimeMinutes} minutos !!!"
                ));
        }

        public void ValidatorCategory()
        {
            RuleFor(recipeDTO => recipeDTO).Custom((recipeDTO, context) =>
            {
                if (!Enum.IsDefined(typeof(ECategory), recipeDTO.Category))                
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(
                                nameof(recipeDTO.Category),
                                string.Format(Resource.ValidatorCategory_Error, recipeDTO.Category, nameof(ValidatorCategory) )));           
            });
        }

        public void ValidatorDeleteImageFile()
        {
            RuleFor(recipeDTO => recipeDTO).Custom((recipeDTO,  context) =>
                {
                    if (recipeDTO.DeleteImageFile && recipeDTO.OldFileExtension == null)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(
                            nameof(recipeDTO.DeleteImageFile), 
                                string.Format(Resource.ValidatorDeleteImageFile_error_WithoutImagem, 
                                                nameof(ValidatorDeleteImageFile))));
                    }
                });
        }
    }
}
