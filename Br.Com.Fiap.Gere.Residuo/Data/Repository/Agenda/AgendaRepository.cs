using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Agenda
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly DatabaseContext _context;

        public AgendaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(AgendaModel agenda)
        {
            _context.TabelaAgenda.Add(agenda);
            _context.SaveChanges();
        }

        public void Delete(AgendaModel agenda)
        {
            _context.TabelaAgenda.Remove(agenda);
            _context.SaveChanges();
        }

        public void Update(AgendaModel agenda)
        {
            _context.TabelaAgenda.Update(agenda);
            _context.SaveChanges();
        }

        public AgendaModel GetById(int id)
        {
            return _context.TabelaAgenda
                .Include(a => a.BairroAgendadoParaColeta)
                .Include(a => a.MotoristaAlocadoParaAgenda)
                .Include(a => a.CaminhaoAlocadoParaAgenda)
                .Include(a => a.NotificacaoGeradaParaEstaAgenda)
                .FirstOrDefault(a => a.AgendaId == id);
        }

        public IEnumerable<AgendaModel> GetAll()
        {
            return _context.TabelaAgenda
                .Include(a => a.BairroAgendadoParaColeta)
                .Include(a => a.MotoristaAlocadoParaAgenda)
                .Include(a => a.CaminhaoAlocadoParaAgenda)
                .Include(a => a.NotificacaoGeradaParaEstaAgenda)
                .ToList();
        }

        public IEnumerable<AgendaModel> GetAll(int page, int size)
        {
            return _context.TabelaAgenda
                .Include(a => a.BairroAgendadoParaColeta)
                .Include(a => a.MotoristaAlocadoParaAgenda)
                .Include(a => a.CaminhaoAlocadoParaAgenda)
                .Include(a => a.NotificacaoGeradaParaEstaAgenda)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();

        }

        public IEnumerable<AgendaModel> GetAllReference(int lastReference, int size)
        {
            var agendas = _context.TabelaAgenda
                .Include(a => a.BairroAgendadoParaColeta)
                .Include(a => a.MotoristaAlocadoParaAgenda)
                .Include(a => a.CaminhaoAlocadoParaAgenda)
                .Include(a => a.NotificacaoGeradaParaEstaAgenda)
                .Where(a => a.AgendaId > lastReference)
                                .OrderBy(a => a.AgendaId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return agendas;

        }

    }
}
