using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao
{
    public interface INotificacaoRepository
    {
        IEnumerable<NotificacaoModel> GetAll();
        NotificacaoModel GetById(int id);
        void Add(NotificacaoModel notificacao);
        void Update(NotificacaoModel notificacao);
        void Delete(NotificacaoModel notificacao);

        // Paginacao por Skip
        IEnumerable<NotificacaoModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<NotificacaoModel> GetAllReference(int lastReference, int size);
    }
}
