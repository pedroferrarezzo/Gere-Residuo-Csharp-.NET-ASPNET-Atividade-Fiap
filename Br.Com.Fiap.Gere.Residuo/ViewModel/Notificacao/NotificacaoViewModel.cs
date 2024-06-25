using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao
{
    public class NotificacaoViewModel
    {
        public int NotificacaoId { get; set; }

        // Relacionamento com AgendaModel
        public AgendaViewModelForResponse AgendaDaNotificacao { get; set; }
    }
}
