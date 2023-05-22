using FluentValidation;
using MeuLivroDeReceitas.CrossCutting.Dto.Ingredient;
using MeuLivroDeReceitas.CrossCutting.EnumClass;
using MeuLivroDeReceitas.CrossCutting.Resources.Application;

namespace MeuLivroDeReceitas.Application.Services
{
    public class IngredientValidator : AbstractValidator<IngredientAddDto>
    {
        public int MinimumNumberOfCharactersInSku = 4;
        public int MaximumNumberOfCharactersInSku = 20; 
        public int MinimumNumberOfCharactersInQuantity = 3;
        public int MaximumNumberOfCharactersInQuantity = 15;

        public IngredientValidator(EMethodIngredientValidator method)
        {
            switch (method)
            {
                case EMethodIngredientValidator.AddIngredient: ValidatorAddIngredient(); return;
            };
        }

        public void ValidatorAddIngredient()
        {
            ValidatorSku();
            ValidatorQuantity();
        }

        public void ValidatorSku()
        {
            RuleFor(c => c.Sku).NotEmpty().WithMessage(string.Format(Resource.ValidatorSku_Error_IsRequired));

            RuleFor(ingrSku => ingrSku.Sku)
                .Length(MinimumNumberOfCharactersInSku, MaximumNumberOfCharactersInSku)
                .WithMessage(ingrSku => string.Format(Resource.ValidatorSku_Error_LengthCharacters,
                ingrSku.Sku.Length,
                MinimumNumberOfCharactersInSku,
                MaximumNumberOfCharactersInSku,
                nameof(ValidatorSku)));
        }
        public void ValidatorQuantity()
        {
            RuleFor(c => c.Quantity).NotEmpty().WithMessage(string.Format(Resource.ValidatorQuantity_Error_IsRequired));

            RuleFor(customer => customer.Quantity)
                .Length(MinimumNumberOfCharactersInQuantity, MaximumNumberOfCharactersInQuantity)
                .WithMessage(customer => string.Format(Resource.ValidatorQuantity_Error_LengthCharacters,
                customer.Quantity.Length,
                MinimumNumberOfCharactersInQuantity,
                MaximumNumberOfCharactersInQuantity,
                nameof(ValidatorQuantity)));
        }
    }
}
