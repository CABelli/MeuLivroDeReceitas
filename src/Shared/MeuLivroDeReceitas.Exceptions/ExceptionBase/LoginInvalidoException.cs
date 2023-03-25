using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using System.Runtime.Serialization;

namespace MeuLivroDeReceitas.Exceptions.ExceptionBase;

[Serializable]
public class LoginInvalidoException : MeuLivroDeReceitasException
{
    public LoginInvalidoException() : base("Login com problema !!!")//ResourceMensagensDeErro.LOGIN_INVALIDO)
    {
    }

    protected LoginInvalidoException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
