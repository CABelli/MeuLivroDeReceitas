using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Request;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;
using System.Text.RegularExpressions;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeValidator : AbstractValidator<RecipeDTO>
    {
        public RecipeValidator()
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledTitle, nameof(RecipeValidator)));
            RuleFor(c => c.PreparationMode).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationMode, nameof(RecipeValidator)));
            RuleFor(c => c.PreparationTime).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledPreparationTime, nameof(RecipeValidator)));
            RuleFor(c => c.Category).NotEmpty().WithMessage(string.Format(Resource.RecipeValidator_Error_UnfilledCategory, nameof(RecipeValidator)));

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
        }
    }
}
