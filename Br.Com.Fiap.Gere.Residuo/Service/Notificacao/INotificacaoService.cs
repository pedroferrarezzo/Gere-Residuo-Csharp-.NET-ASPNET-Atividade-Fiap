using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Notificacao
{
    public interface INotificacaoService
    {
        IEnumerable<NotificacaoModel> ListarNotificacoes();
        NotificacaoModel ObterNotificacaoPorId(int id);
        void CriarNotificacao(NotificacaoModel notificacao);
        void AtualizarNotificacao(NotificacaoModel notificacao);
        void DeletarNotificacao(int id);

        IEnumerable<NotificacaoModel> ListarNotificacoesPaginacaoReference(int lastReference, int size);

        public IEnumerable<NotificacaoModel> ListarNotificacoesPaginacaoPage(int page, int size);

    }
}
