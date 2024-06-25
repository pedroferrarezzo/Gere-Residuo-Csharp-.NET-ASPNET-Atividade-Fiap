using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao
{
    public class NotificacaoViewModelComPaginacaoReference
    {
        public IEnumerable<NotificacaoViewModel> Notificacoes { get; set; }
        public int PageSize { get; set; }
        public int Ref { get; set; }
        public int NextRef { get; set; }
        public string PreviousPageUrl => $"/Agenda?referencia={Ref}&tamanho={PageSize}";
        public string NextPageUrl => (Ref < NextRef) ? $"/Agenda?referencia={NextRef}&tamanho={PageSize}" : "";
    }
}
