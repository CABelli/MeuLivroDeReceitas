using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Login;
using MeuLivroDeReceitas.CrossCutting.Dto.Request.Login;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;
using MeuLivroDeReceitas.Infrastructure.Identity;

namespace MeuLivroDeReceitas.Infrastructure.IdentityValidator
{
    public class UserValidator : AbstractValidator<UserValidatorDto>
    {
        public int MinimumNumberOfCharactersInPassword = 10;
        public int MaximumNumberOfCharactersInPassword = 20;

        public UserValidator(int charactersTitle, MethodUserValidator method)
        {            
            switch (method)
            {
                case MethodUserValidator.Authenticate: ValidatorAuthenticate(charactersTitle); return;
                case MethodUserValidator.AddUser: ValidatorAddUser(charactersTitle); return;
                case MethodUserValidator.UserChange: ValidatorUserChange(); return;
                case MethodUserValidator.PasswordChangeByForgot: ValidatorPasswordChangeByForgot(charactersTitle); return;                    
            };
        }

        public void ValidatorAuthenticate(int charactersTitle)
        {
            ValidatorUserName();
            ValidatorPassword(charactersTitle);
        }

        public void ValidatorAddUser(int charactersTitle)
        {
            ValidatorUserName();
            ValidatorPhoneNumber();
            ValidatorPassword(charactersTitle);
            ValidatorEmail();
        }

        public void ValidatorUserChange()
        {
            ValidatorPhoneNumber();
        }

        public void ValidatorPasswordChangeByForgot(int charactersTitle)
        {
            ValidatorPassword(charactersTitle);
            ValidatorRolesName();
        }

        public void ValidatorUserName()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage(string.Format(Resource.UserValidator_Error_UserNameIsRequired));
        }

        public void ValidatorPassword(int charactersTitle)
        {
            RuleFor(c => c.Password).NotEmpty().WithMessage(string.Format(Resource.LoginValidator_Error_PasswordIsRequired));

            RuleFor(c => c.Password).Length(MinimumNumberOfCharactersInPassword, MaximumNumberOfCharactersInPassword)
                .WithMessage(string.Format(Resource.LoginValidator_Error_CharactersPassword,
                charactersTitle,
                MinimumNumberOfCharactersInPassword,
                MaximumNumberOfCharactersInPassword));
        }

        public void ValidatorPhoneNumber()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty()
                .WithMessage(string.Format(Resource.UserValidator_Error_PhoneNumberIsRequired));

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
        }

        public void ValidatorEmail()
        {
            RuleFor(c => c.Email).NotEmpty()
                .WithMessage(string.Format(Resource.LoginValidator_Error_EmailIsRequired));

            When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
            {
                RuleFor(c => c.Email).EmailAddress().WithMessage(Resource.LoginValidator_Error_EmailIsInvalid);
            });
        }

        public void ValidatorRolesName()
        {
            RuleFor(c => c.RolesName).NotEmpty().WithMessage(string.Format("Somente Admin pode trocar senha"));
           // RuleFor(c => c.RolesName).NotEqual("Admin").WithMessage(string.Format("Somente Admin pode trocar senha"));

            // .Must(x => x.All(x =>

            RuleFor(c => c.RolesName)
               // .Must(x => x.All().NotEqual("Admin")
                .Must(x => x.All(x => !x.Equals("Admin"))) //NotEqual("Admin"))
                .WithMessage(string.Format("Somente Admin pode trocar senha"));
        }
    }
}
