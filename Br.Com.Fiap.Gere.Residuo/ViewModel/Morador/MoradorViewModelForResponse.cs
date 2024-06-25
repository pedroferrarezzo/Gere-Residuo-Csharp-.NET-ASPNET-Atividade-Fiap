using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Morador
{
    public class MoradorViewModelForResponse
    {
        public int MoradorId { get; set; }

        // Relacionamento com BairroModel
        public int BairroId { get; set; }

        public string MoradorNome { get; set; }

        public string MoradorEmail { get; set; }

    }
}
