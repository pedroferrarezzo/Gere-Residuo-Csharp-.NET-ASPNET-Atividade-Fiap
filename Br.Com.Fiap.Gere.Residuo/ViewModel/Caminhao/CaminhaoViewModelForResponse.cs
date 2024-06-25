using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao
{
    public class CaminhaoViewModelForResponse
    {
        public int CaminhaoId { get; set; }

        public string CaminhaoPlaca { get; set; }
        public DateOnly DataFabricacao { get; set; }

        public string CaminhaoMarca { get; set; }

        public string CaminhaoModelo { get; set; }

        public bool CaminhaoEstaDisponivel { get; set; }

    }
}
