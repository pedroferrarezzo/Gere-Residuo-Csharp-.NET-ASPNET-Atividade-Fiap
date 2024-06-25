using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Agenda
{
    public interface IAgendaRepository
    {
        IEnumerable<AgendaModel> GetAll();
        AgendaModel GetById(int id);
        void Add(AgendaModel agenda);
        void Update(AgendaModel agenda);
        void Delete(AgendaModel agenda);

        // Paginacao por Skip
        IEnumerable<AgendaModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<AgendaModel> GetAllReference(int lastReference, int size);
    }
}
