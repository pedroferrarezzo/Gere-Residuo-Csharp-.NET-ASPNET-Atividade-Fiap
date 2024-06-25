using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista
{
    public class MotoristaViewModel
    {
        public int MotoristaId { get; set; }

        public string MotoristaNome { get; set; }

        public string MotoristaCpf { get; set; }

        public string MotoristaNrCelular { get; set; }

        public string MotoristaNrCelularDdd { get; set; }

        public string MotoristaNrCelularDdi { get; set; }

        public bool MotoristaEstaDisponivel { get; set; }
        // Relacionamento com AgendaModel
        public List<AgendaViewModelForResponse> AgendasCriadasComEsteMotorista { get; set; }

    }
}
