using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista
{
    public class MotoristaViewModelComPaginacaoReference
    {
        public IEnumerable<MotoristaViewModel> Motoristas { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Motorista?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Motorista?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
