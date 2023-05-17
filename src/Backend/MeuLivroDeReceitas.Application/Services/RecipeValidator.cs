using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Recipess;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeValidator : AbstractValidator<RecipeDTO>
    {
        public int MinimumNumberOfCharactersInTitle = 5;
        public int MaximumNumberOfCharactersInTitle = 20;

        public RecipeValidator(MethodRecipeValidator method)
        {
            switch (method)
            {
                case MethodRecipeValidator.AddRecipe: ValidatorAddRecipe(); return;
                case MethodRecipeValidator.ModifyRecipe: ValidatorModifyRecipe(); return;
            };
        }

        public void ValidatorAddRecipe()
        {
            ValidatorTitle();
            ValidatorPreparationMode();
            ValidatorPreparationTime();
            ValidatorPreparationCategory();
        }

        public void ValidatorModifyRecipe()
        {
            ValidatorTitle();
            ValidatorPreparationMode();
            ValidatorPreparationTime();
            ValidatorPreparationCategory();
            ValidatorFileExtension();
        }

        public void ValidatorTitle()
        {
            RuleFor(c => c.Title).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledTitle, nameof(RecipeValidator)));

            RuleFor(RecTile => RecTile.Title).Length(MinimumNumberOfCharactersInTitle, MaximumNumberOfCharactersInTitle)
                .WithMessage(RecTile => string.Format(Resource.RecipeValidator_Error_CharactersTitle,
                RecTile.Title.Length,
                MinimumNumberOfCharactersInTitle,
                MaximumNumberOfCharactersInTitle,
                nameof(RecipeValidator)));
        }

        public void ValidatorPreparationMode()
        {
            RuleFor(c => c.PreparationMode).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationMode, nameof(RecipeValidator)));
        }

        public void ValidatorPreparationTime()
        {
            RuleFor(c => c.PreparationTime).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationTime, nameof(RecipeValidator)));
        }

        public void ValidatorPreparationCategory()
        {
            RuleFor(c => c.CategoryRecipe).NotEmpty()
                .WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledCategory, nameof(RecipeValidator)));
        }

        public void ValidatorFileExtension()
        {
            //RuleFor(c => c.FileExtension).Empty().When(c => c.DataDraft == null || c.DataDraft == "")
                //.WithMessage(string.Format(Resource.RecipeValidator_Error_DataDraftIsNull, nameof(RecipeValidator)));

            //RuleFor(c => c.DataDraft).Empty().When(c => c.FileExtension == null || c.FileExtension == "")
                //.WithMessage(string.Format(Resource.RecipeValidator_Error_FileExtensionIsNull, nameof(RecipeValidator)));
        }
    }
}
