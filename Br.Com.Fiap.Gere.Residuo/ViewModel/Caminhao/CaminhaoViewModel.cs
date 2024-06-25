using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao
{
    public class CaminhaoViewModel
    {
        public int CaminhaoId { get; set; }

        public string CaminhaoPlaca { get; set; }
        public DateOnly DataFabricacao { get; set; }

        public string CaminhaoMarca { get; set; }

        public string CaminhaoModelo { get; set; }

        public bool CaminhaoEstaDisponivel { get; set; }

        // Relacionamento com AgendaModel
        public List<AgendaViewModelForResponse> AgendasCriadasComEsteCaminhao { get; set; }

    }
}
