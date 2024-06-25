using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Morador
{
    public class MoradorViewModel
    {
        public int MoradorId { get; set; }

        public string MoradorNome { get; set; }

        public string MoradorEmail { get; set; }

        // Relacionamento com BairroModel
        public BairroViewModelForResponse BairroDoMorador { get; set; }

    }
}
