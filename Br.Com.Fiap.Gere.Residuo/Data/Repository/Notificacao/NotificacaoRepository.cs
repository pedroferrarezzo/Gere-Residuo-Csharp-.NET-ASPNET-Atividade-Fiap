using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly DatabaseContext _context;

        public NotificacaoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(NotificacaoModel notificacao)
        {
            _context.TabelaNotificacao.Add(notificacao);
            _context.SaveChanges();
        }

        public void Delete(NotificacaoModel notificacao)
        {
            _context.TabelaNotificacao.Remove(notificacao);
            _context.SaveChanges();
        }

        public void Update(NotificacaoModel notificacao)
        {
            _context.TabelaNotificacao.Update(notificacao);
            _context.SaveChanges();
        }

        public NotificacaoModel GetById(int id)
        {
            return _context.TabelaNotificacao
                .Include(n => n.AgendaDaNotificacao)
                .ThenInclude(a => a.BairroAgendadoParaColeta)
                .ThenInclude(b => b.MoradoresDoBairro)
                .FirstOrDefault(n => n.NotificacaoId == id);

        }

        public IEnumerable<NotificacaoModel> GetAll()
        {
            return _context.TabelaNotificacao
                .Include(n => n.AgendaDaNotificacao)
                .ToList();
        }

        public IEnumerable<NotificacaoModel> GetAll(int page, int size)
        {
            return _context.TabelaNotificacao
                .Include(n => n.AgendaDaNotificacao)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<NotificacaoModel> GetAllReference(int lastReference, int size)
        {
            var notificacoes = _context.TabelaNotificacao
                .Include(n => n.AgendaDaNotificacao)
                .Where(n => n.NotificacaoId > lastReference)
                                .OrderBy(n => n.NotificacaoId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return notificacoes;
        }
    }
}
