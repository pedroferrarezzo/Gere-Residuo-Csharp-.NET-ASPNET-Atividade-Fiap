using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro
{
    public class BairroRepository : IBairroRepository
    {
        private readonly DatabaseContext _context;

        public BairroRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(BairroModel bairro)
        {
            _context.TabelaBairro.Add(bairro);
            _context.SaveChanges();
        }

        public void Delete(BairroModel bairro)
        {
            _context.TabelaBairro.Remove(bairro);
            _context.SaveChanges();
        }

        public void Update(BairroModel bairro)
        {
            _context.TabelaBairro.Update(bairro);
            _context.SaveChanges();
        }

        public void UpdateBairroEstaDisponivel(int bairroId, bool disponibilidade)
        {
            var bairro = this.GetById(bairroId);
            bairro.BairroEstaDisponivel = disponibilidade;
            _context.SaveChanges();
           
        }

        public void UpdatePercentualColetaLixoBairro(int bairroId, int percentualColetaLixoBairro)
        {
            var bairro = this.GetById(bairroId);
            bairro.PercentualColetaLixoBairro = percentualColetaLixoBairro;
            _context.SaveChanges();

        }

        public BairroModel GetById(int id)
        {
            return _context.TabelaBairro
                .Include(b => b.MoradoresDoBairro)
                .Include(b => b.AgendasDeColetaDeLixoDoBairro)
                .FirstOrDefault(b => b.BairroId == id);

        }

        public bool GetBairroEstaDisponivel(int id)
        {
            return _context.TabelaBairro.Find(id).BairroEstaDisponivel;

        }

        public IEnumerable<BairroModel> GetAll()
        {
            return _context.TabelaBairro
                .Include(b => b.MoradoresDoBairro)
                .Include(b => b.AgendasDeColetaDeLixoDoBairro)
                .ToList();
        }

        public IEnumerable<BairroModel> GetAll(int page, int size)
        {
            return _context.TabelaBairro
                .Include(b => b.MoradoresDoBairro)
                .Include(b => b.AgendasDeColetaDeLixoDoBairro)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<BairroModel> GetAllReference(int lastReference, int size)
        {
            var bairros = _context.TabelaBairro
                .Include(b => b.MoradoresDoBairro)
                .Include(b => b.AgendasDeColetaDeLixoDoBairro)
                .Where(b => b.BairroId > lastReference)
                                .OrderBy(b => b.BairroId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return bairros;
        }

    }
}
