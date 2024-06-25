using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao
{
    public class NotificacaoViewModelForResponse
    {
        public int NotificacaoId { get; set; }

        // Relacionamento com AgendaModel
        public int AgendaId { get; set; }
    }
}
