using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;

namespace MeuLivroDeReceitas.Infrastructure.Identity
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public int MinimumNumberOfCharactersInPassword = 10;
        public int MaximumNumberOfCharactersInPassword = 20;

        public LoginValidator(int CharactersTitle)
        {
            RuleFor(c => c.Email).NotEmpty()
                .WithMessage(string.Format(Resource.LoginValidator_Error_EmailIsRequired));

            When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
            {
                RuleFor(c => c.Email).EmailAddress().WithMessage(Resource.LoginValidator_Error_EmailIsInvalid);
            });

            RuleFor(c => c.Password).NotEmpty()
                .WithMessage(string.Format(Resource.LoginValidator_Error_PasswordIsRequired));
            
            RuleFor(c => c.Password).Length(MinimumNumberOfCharactersInPassword, MaximumNumberOfCharactersInPassword)
                .WithMessage(string.Format(Resource.LoginValidator_Error_CharactersPassword,
                CharactersTitle,
                MinimumNumberOfCharactersInPassword,
                MaximumNumberOfCharactersInPassword));
        }
    }
}
