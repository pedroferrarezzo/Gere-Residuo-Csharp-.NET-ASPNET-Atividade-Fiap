using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Morador
{
    public class MoradorRepository : IMoradorRepository
    {
        private readonly DatabaseContext _context;

        public MoradorRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(MoradorModel morador)
        {
            _context.TabelaMorador.Add(morador);
            _context.SaveChanges();
        }

        public void Delete(MoradorModel morador)
        {
            _context.TabelaMorador.Remove(morador);
            _context.SaveChanges();
        }

        public void Update(MoradorModel morador)
        {
            _context.TabelaMorador.Update(morador);
            _context.SaveChanges();
        }

        public MoradorModel GetById(int id)
        {
            return _context.TabelaMorador
                .Include(m => m.BairroDoMorador)
                .FirstOrDefault(m => m.MoradorId == id);

        }

        public IEnumerable<MoradorModel> GetByBairroId(int bairroId)
        {
            return _context.TabelaMorador.Where(m => m.BairroId == bairroId);
        }

        public IEnumerable<MoradorModel> GetAll()
        {
            return _context.TabelaMorador
                .Include(m => m.BairroDoMorador)
                .ToList();
        }

        public IEnumerable<MoradorModel> GetAll(int page, int size)
        {
            return _context.TabelaMorador
                .Include(m => m.BairroDoMorador)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<MoradorModel> GetAllReference(int lastReference, int size)
        {
            var moradores = _context.TabelaMorador
                .Include(m => m.BairroDoMorador)
                .Where(m => m.MoradorId > lastReference)
                                .OrderBy(m => m.MoradorId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return moradores;
        }
    }
}
