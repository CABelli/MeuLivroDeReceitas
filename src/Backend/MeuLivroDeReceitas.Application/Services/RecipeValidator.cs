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
            ValidatorCategory();
        }

        public void ValidatorModifyRecipe()
        {
            ValidatorTitle();
            ValidatorPreparationMode();
            ValidatorPreparationTime();
            ValidatorCategory();
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

        public void ValidatorCategory()
        {
            RuleFor(recipeDTO => recipeDTO).Custom((recipeDTO, context) =>
            {
                if (!Enum.IsDefined(typeof(Category), recipeDTO.CategoryRecipe))                
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(
                                nameof(recipeDTO.CategoryRecipe),
                                string.Format(Resource.ValidatorCategory_Error, recipeDTO.CategoryRecipe, nameof(ValidatorCategory) )));           
            });
        }

        public void ValidatorFileExtension()
        {
            // RecipeValidator_Error_DataDraftIsNull
            // RecipeValidator_Error_FileExtensionIsNull

            RuleFor(c => c).Custom((custom,  context) =>
                { 
                    if (custom.FileExtension != custom.OldFileExtension)
                    {
                        if (custom.OldFileExtension == null)
                        {
                            context.AddFailure(new FluentValidation.Results.ValidationFailure(
                                nameof(custom.FileExtension), "Extensão nova invalida porque a receita não contem imagem"));
                            
                            // Receita sem imagem não pode conter extensão de arquivo
                            // Recipe without image cannot contain file extension

                            // ValidatorFileExtension_error_ExtensionWithoutImagem
                        }
                        else
                        {
                            if (custom.FileExtension != null)
                            {
                                context.AddFailure(new FluentValidation.Results.ValidationFailure(
                                    nameof(custom.FileExtension), 
                                    string.Format($"Extensão nova: {custom.FileExtension} não pode ser alterada, atual: {custom.OldFileExtension}")));

                                // "Extensão: {0} não pode ser alterada, já existe a extensão: {1}

                                // ValidatorFileExtension_error_ExtensionNotUpdate
                            }
                        }
                    }
            });
        }
    }
}
