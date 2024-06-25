using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario
{
    public class UsuarioViewModelComPaginacaoReference
    {
        public IEnumerable<UsuarioViewModel> Usuarios { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Usuario?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Usuario?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
