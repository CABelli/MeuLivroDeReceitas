using MeuLivroDeReceitas.CrossCutting.Dto.Response;
using MeuLivroDeReceitas.CrossCutting.Resources.API;
using MeuLivroDeReceitas.Exceptions.ExceptionBase;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web;

namespace MeuLivroDeReceitas.Api.Filtros;

public class FiltroDasExceptions : IExceptionFilter
{
    private readonly ILogger<FiltroDasExceptions> _logger;

    public FiltroDasExceptions(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<FiltroDasExceptions>>();
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MeuLivroDeReceitasException)
        {
            OutPutLogger(context);
            TratarMeuLivroDeReceitasException(context);
        }
        else
        {
            _logger.LogError(string.Format(Resource.OnException_Error_UnknownShortMessage, nameof(OnException), context.Exception.Message));
            _logger.LogError(string.Format(Resource.OnException_Error_UnknownCompleteMessage, nameof(OnException), context.Exception.StackTrace));
            ThrowUnknownError(context);
        }
    }

    private void OutPutLogger(ExceptionContext context)
    {
        if (context.Exception is ErrosDeValidacaoException)
        {
            var validationErros = context.Exception as ErrosDeValidacaoException;
            validationErros?.MensagensDeErro.ForEach(mensageError => _logger.LogInformation(mensageError));
        }
        else if (context.Exception is ErrorsNotFoundException)
        {
            var notFoundErros = context.Exception as ErrorsNotFoundException;
            notFoundErros?.MensagensDeErro.ForEach(mensageError => _logger.LogInformation(mensageError));
        }
    }

    private static void TratarMeuLivroDeReceitasException(ExceptionContext context)
    {
        if (context.Exception is ErrosDeValidacaoException)        
            TratarErrosDeValidacaoException(context);
        
        else if (context.Exception is ErrorsNotFoundException)        
            TratarErrorsNotFoundException(context); 
    }

    private static void TratarErrosDeValidacaoException(ExceptionContext context)
    {
        var erroDeValidacaoException = context.Exception as ErrosDeValidacaoException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new RespostaErroJson(erroDeValidacaoException.MensagensDeErro));
    }

    private static void TratarErrorsNotFoundException(ExceptionContext context)
    {
        var errorsNotFoundException = context.Exception as ErrorsNotFoundException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
        context.HttpContext.Response.Headers.Add("Reason", HttpUtility.HtmlEncode(errorsNotFoundException.MensagensDeErro.FirstOrDefault()));
        context.Result = new ObjectResult("");
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new RespostaErroJson(string.Format(Resource.ThrowUnknownError_Error_Throw, nameof(ThrowUnknownError), context.Exception.Message)));
    }
}