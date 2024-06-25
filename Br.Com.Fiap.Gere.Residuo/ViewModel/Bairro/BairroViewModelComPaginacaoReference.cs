using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro
{
    public class BairroViewModelComPaginacaoReference
    {
        public IEnumerable<BairroViewModel> Bairros { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Bairro?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Bairro?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
