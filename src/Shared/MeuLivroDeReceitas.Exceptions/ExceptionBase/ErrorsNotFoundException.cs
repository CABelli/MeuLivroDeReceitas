using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using System.Runtime.Serialization;

namespace MeuLivroDeReceitas.Exceptions.ExceptionBase
{
    [Serializable]
    public class ErrorsNotFoundException : MeuLivroDeReceitasException
    {
        public List<string> MensagensDeErro { get; set; }

        public ErrorsNotFoundException(List<string> mensagensDeErro) : base(string.Empty)
        {
            MensagensDeErro = mensagensDeErro;
        }

        protected ErrorsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}