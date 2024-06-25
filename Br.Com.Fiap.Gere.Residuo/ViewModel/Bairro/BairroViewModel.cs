using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro
{
    public class BairroViewModel
    {
        public int BairroId { get; set; }

        public string BairroNome { get; set; }

        public int QuantidadeLixeiras { get; set; }

        public long PesoMedioLixeirasKg { get; set; }

        public int PercentualColetaLixoBairro { get; set; }

        public bool BairroEstaDisponivel { get; set; }

        // Relacionamento com MoradorModel
        public List<MoradorViewModelForResponse> MoradoresDoBairro { get; set; }

        // Relacionamento com AgendaModel
        public List<AgendaViewModelForResponse> AgendasDeColetaDeLixoDoBairro { get; set; }
    }
}
