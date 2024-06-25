namespace Br.Com.Fiap.Gere.Residuo.Exception
{
    public class CaminhaoNaoEstaDisponivelException : System.Exception
    {

        public CaminhaoNaoEstaDisponivelException()
        {
        }

        public CaminhaoNaoEstaDisponivelException(string message)
            : base(message)
        {
        }

        public CaminhaoNaoEstaDisponivelException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

    }
   
}

