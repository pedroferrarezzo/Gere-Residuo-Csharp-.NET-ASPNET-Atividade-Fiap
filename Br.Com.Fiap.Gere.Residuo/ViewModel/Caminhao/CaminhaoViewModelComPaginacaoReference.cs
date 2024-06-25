using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao
{
    public class CaminhaoViewModelComPaginacaoReference
    {
        public IEnumerable<CaminhaoViewModel> Caminhoes { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Caminhao?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Caminhao?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
