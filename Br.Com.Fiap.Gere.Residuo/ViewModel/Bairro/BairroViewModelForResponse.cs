using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro
{
    public class BairroViewModelForResponse
    {
        public int BairroId { get; set; }

        public string BairroNome { get; set; }

        public int QuantidadeLixeiras { get; set; }

        public long PesoMedioLixeirasKg { get; set; }

        public int PercentualColetaLixoBairro { get; set; }

        public bool BairroEstaDisponivel { get; set; }
    }
}
