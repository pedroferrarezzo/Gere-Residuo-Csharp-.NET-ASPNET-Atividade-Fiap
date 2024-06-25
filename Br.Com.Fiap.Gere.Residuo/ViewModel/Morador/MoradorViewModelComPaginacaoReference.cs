using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Morador
{
    public class MoradorViewModelComPaginacaoReference
    {
        public IEnumerable<MoradorViewModel> Moradores { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Morador?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Morador?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
