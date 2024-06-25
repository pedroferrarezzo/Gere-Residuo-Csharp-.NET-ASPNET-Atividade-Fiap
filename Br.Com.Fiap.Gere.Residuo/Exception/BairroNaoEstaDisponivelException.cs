namespace Br.Com.Fiap.Gere.Residuo.Exception
{
    public class BairroNaoEstaDisponivelException : System.Exception
    {

        public BairroNaoEstaDisponivelException()
        {
        }

        public BairroNaoEstaDisponivelException(string message)
            : base(message)
        {
        }

        public BairroNaoEstaDisponivelException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

    }
   
}

