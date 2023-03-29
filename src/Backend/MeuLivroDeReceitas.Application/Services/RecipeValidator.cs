using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
using System.Text.RegularExpressions;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeValidator : AbstractValidator<RecipeDTO>
    {
        public RecipeValidator(int action)
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledTitle, nameof(RecipeValidator)));
            if (action == 1) RuleFor(c => c.PreparationMode).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationMode, nameof(RecipeValidator)));
            if (action == 1) RuleFor(c => c.PreparationTime).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationTime, nameof(RecipeValidator)));
            if (action == 1) RuleFor(c => c.Category).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledCategory, nameof(RecipeValidator)));

            When(c => c.FileExtension == "Cel" && 
            !string.IsNullOrWhiteSpace(c.DataDraft), () =>
            {
                RuleFor(c => c.DataDraft).Custom((cellNumber, context) =>
                {
                    string standardCell = "([0-9]{2,3})?([0-9]{2})([0-9]{4,5})([0-9]{4})"; //"([0-9]{2})[1-9]{1} [0-9]{4}-[0-9]{4}";
                    var isMatch = Regex.IsMatch(cellNumber, standardCell);                    
                    if (!isMatch)
                    {
                        context.AddFailure(
                            new FluentValidation.Results.ValidationFailure(nameof(cellNumber), 
                            string.Format(Resource.RecipeValidator_Error_NonStandardCellNumber, nameof(RecipeValidator), cellNumber) ));
                    }
                });
            });


            RuleFor(c => c.FileExtension).Empty().When(c => c.DataDraft == null).WithMessage("DataDraft -- is null");
            RuleFor(c => c.DataDraft).Empty().When(c => c.FileExtension == null).WithMessage("FileExtension -- is null");

            RuleFor(c => c.FileExtension).NotEmpty().When(c => c.DataDraft != null).WithMessage("FileExtension 02 is null");
            RuleFor(c => c.FileExtension != null).NotEmpty().When(c => c.DataDraft == null).WithMessage("DataDraft 02 is null");

            //if (recipeDTO.FileExtension == null && recipeDTO.DataDraft != null) await Task.FromException<Exception>(new Exception("FileExtension is null"));
            //if (recipeDTO.FileExtension != null && recipeDTO.DataDraft == null) await Task.FromException<Exception>(new Exception("DataDraft is null"));
        }
    }
}
