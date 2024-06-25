namespace Br.Com.Fiap.Gere.Residuo.Exception
{
    public class AgendaJaEstaFinalizadaException : System.Exception
    {

        public AgendaJaEstaFinalizadaException()
        {
        }

        public AgendaJaEstaFinalizadaException(string message)
            : base(message)
        {
        }

        public AgendaJaEstaFinalizadaException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

    }
   
}

