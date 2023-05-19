using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Login;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Extensions;
using MeuLivroDeReceitas.CrossCutting.Resources.Infrastructure;

namespace MeuLivroDeReceitas.Infrastructure.IdentityValidator
{
    public class UserValidator : AbstractValidator<UserValidatorDto>
    {
        public int MinimumNumberOfCharactersInPassword = 10;
        public int MaximumNumberOfCharactersInPassword = 20;

        public UserValidator(MethodUserValidator method)
        {            
            switch (method)
            {
                case MethodUserValidator.Authenticate: ValidatorAuthenticate(); return;
                case MethodUserValidator.AddUser: ValidatorAddUser(); return;
                case MethodUserValidator.UserChange: ValidatorUserChange(); return;
                case MethodUserValidator.PasswordChangeByForgot: ValidatorPasswordChangeByForgot(); return;                    
            };
        }

        public void ValidatorAuthenticate()
        {
            ValidatorUserName();
            ValidatorPassword();
        }

        public void ValidatorAddUser()
        {
            ValidatorUserName();
            ValidatorPhoneNumber();
            ValidatorPassword();
            ValidatorEmail();
        }

        public void ValidatorUserChange()
        {
            ValidatorPhoneNumber();
        }

        public void ValidatorPasswordChangeByForgot()
        {
            ValidatorPassword();
            ValidatorPasswordRepeatNewPassword();
            ValidatorRolesName();
        }

        public void ValidatorUserName()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage(string.Format(Resource.UserValidator_Error_UserNameIsRequired));
        }

        public void ValidatorPassword()
        {
            RuleFor(c => c.Password).NotEmpty().WithMessage(string.Format(Resource.ValidatorPassword_Error_PasswordIsRequired));
            
            RuleFor(customer => customer.Password)
                .Length(MinimumNumberOfCharactersInPassword, MaximumNumberOfCharactersInPassword)
                .WithMessage(customer => string.Format(Resource.ValidatorPassword_Error_CharactersPassword,               
                customer.Password.StringLengthText(),
                MinimumNumberOfCharactersInPassword,
                MaximumNumberOfCharactersInPassword));

            //RuleFor.Custom(c => c.PhoneNumber).NotEmpty().WithMessage(" erro ");

            //When(c => c.Password != null, () =>
            //{
            //    RuleFor(c => c.Password).Custom((password, context) =>
            //    {
            //        var passwordNew = " ==1 " + password;
            //        context.AddFailure(new FluentValidation.Results.ValidationFailure(
            //            nameof(password), string.Format(passwordNew)));
            //    });
            //});

            //RuleFor(c => c.Password)
            //    .Length(MinimumNumberOfCharactersInPassword, MaximumNumberOfCharactersInPassword)
            //    .Custom((cellNumber, context) =>
            //    {
            //        var returnValidatorPhone = cellNumber.ValidatorPhone();
            //        if (!returnValidatorPhone)
            //            context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(cellNumber),
            //                            string.Format(Resource.UserChangeValidator_Error_NonStandardCellNumber,
            //                            nameof(UserChangeValidator), cellNumber)));
            //    });
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
                                            string.Format(Resource.ValidatorPhoneNumber_Error_NonStandardCellNumber,
                                            nameof(ValidatorPhoneNumber), cellNumber)));
                });
            });
        }

        public void ValidatorEmail()
        {
            RuleFor(c => c.Email).NotEmpty()
                .WithMessage(string.Format(Resource.ValidatorEmail_Error_EmailIsRequired));

            When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
            {
                RuleFor(c => c.Email).EmailAddress().WithMessage(Resource.ValidatorEmail_Error_EmailIsInvalid);
            });
        }

        public void ValidatorPasswordRepeatNewPassword()
        {
            RuleFor(c => c.Password == c.RepeatNewPassword).Equal(true).WithMessage(Resource.ValidatorPasswordRepeatNewPassword_error_different);
        }

        public void ValidatorRolesName()
        {
            RuleForEach(c => c.RolesName).NotEmpty().WithMessage(string.Format(Resource.ValidatorRolesName_Error_Empty));
            
            RuleForEach(c => c.RolesName).Equal("Admin").WithMessage(string.Format(Resource.ValidatorRolesName_Error_NotAdmin));           

            RuleForEach(x => x.RolesName)
            .Equal("Admin")
            .WithMessage((rolesName, context) => string.Format(Resource.ValidatorRolesName_Error_NotAdminList, 
                                                                rolesName.RolesName.FirstOrDefault()));
           
            RuleForEach(x => x.RolesName)
            .ChildRules(rolesName =>
            {
                rolesName.RuleFor(rolesName => rolesName)
                    .NotNull()
                    .NotEmpty()
                    .Equal("Admin")
                    .WithMessage(rolesName => string.Format(Resource.ValidatorRolesName_Error_NotAdminList, rolesName));
                    //.WithMessage(rolesName => $"002 - RolesName:  {rolesName} Somente Admin pode trocar senha");
            });

            When(c => c.RolesName.FirstOrDefault() != "Admin", () =>
            {
                RuleForEach(c => c.RolesName).Custom((rolesName, context) =>
                {
                    var text = string.Format(Resource.ValidatorRolesName_Error_NotAdminList, rolesName);
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(rolesName), 
                                        string.Format(Resource.ValidatorRolesName_Error_NotAdminList, rolesName)));
                });
            });
        }
    }
}
