using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class NotificacaoModel
    {
        public int NotificacaoId { get; set; }

        // Relacionamento com AgendaModel
        public int AgendaId { get; set; }
        public AgendaModel AgendaDaNotificacao { get; set; }
    }
}
