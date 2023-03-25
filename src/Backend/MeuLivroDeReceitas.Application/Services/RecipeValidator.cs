using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Dto.Request;

namespace MeuLivroDeReceitas.Application.Services
{
    public class RecipeValidator : AbstractValidator<RecipeDTO>
    {
        public RecipeValidator()
        {
            //RuleFor(c => c.Nome).NotEmpty().WithMessage(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO);
            RuleFor(c => c.Title).NotEmpty().WithMessage(" Recipe Title is null ");
        }        
    }
}
