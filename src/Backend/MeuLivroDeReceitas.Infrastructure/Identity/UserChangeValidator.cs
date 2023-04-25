using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using System.Text.RegularExpressions;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;
using MeuLivroDeReceitas.CrossCutting.Extensions;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class UserChangeValidator : AbstractValidator<UserChangeDto>
    {
        public UserChangeValidator()
        {
            When(c => !string.IsNullOrWhiteSpace(c.PhoneNumber), () =>
            {
                RuleFor(c => c.PhoneNumber).Custom((cellNumber, context) =>
                {
                    var returnValidatorPhone = cellNumber.ValidatorPhone();
                    if (!returnValidatorPhone)                    
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(cellNumber),
                                            string.Format(Resource.UserChangeValidator_Error_NonStandardCellNumber,
                                            nameof(UserChangeValidator), cellNumber)));                    
                });
            });

            //Exemplo ilustrativo 
            When(c => !string.IsNullOrWhiteSpace(c.PhoneNumber), () =>
            {
                RuleFor(c => c.PhoneNumber).Custom((cellNumber, context) =>
                {
                    string standardCell = "([0-9]{2,3})?([0-9]{2})([0-9]{4,5})([0-9]{4})";
                    var match = Regex.Match(cellNumber, standardCell);
                    if (! match.Value.Equals(cellNumber))                    
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(cellNumber),
                                            string.Format(Resource.UserChangeValidator_Error_NonStandardCellNumber, 
                                            nameof(UserChangeValidator), cellNumber)));                    
                });
            });
        }
    }
}

