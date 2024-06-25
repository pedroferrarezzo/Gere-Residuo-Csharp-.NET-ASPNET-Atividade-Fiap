using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista
{
    public class MotoristaViewModelForResponse
    {
        public int MotoristaId { get; set; }

        public string MotoristaNome { get; set; }

        public string MotoristaCpf { get; set; }

        public string MotoristaNrCelular { get; set; }

        public string MotoristaNrCelularDdd { get; set; }

        public string MotoristaNrCelularDdi { get; set; }

        public bool MotoristaEstaDisponivel { get; set; }

    }
}
