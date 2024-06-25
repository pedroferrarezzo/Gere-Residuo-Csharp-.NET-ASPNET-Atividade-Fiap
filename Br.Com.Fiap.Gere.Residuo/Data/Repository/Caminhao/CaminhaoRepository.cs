using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao
{
    public class CaminhaoRepository : ICaminhaoRepository
    {
        private readonly DatabaseContext _context;

        public CaminhaoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(CaminhaoModel caminhao)
        {
            _context.TabelaCaminhao.Add(caminhao);
            _context.SaveChanges();
        }

        public void Delete(CaminhaoModel caminhao)
        {
            _context.TabelaCaminhao.Remove(caminhao);
            _context.SaveChanges();
        }

        public void Update(CaminhaoModel caminhao)
        {
            _context.TabelaCaminhao.Update(caminhao);
            _context.SaveChanges();
        }

        public void UpdateCaminhaoEstaDisponivel(int caminhaoId, bool disponibilidade)
        {
            var caminhao = this.GetById(caminhaoId);
            caminhao.CaminhaoEstaDisponivel = disponibilidade;
            _context.SaveChanges();
        }

        public CaminhaoModel GetById(int id)
        {
            return _context.TabelaCaminhao
                .Include(c=> c.AgendasCriadasComEsteCaminhao)
                .FirstOrDefault(c=> c.CaminhaoId == id);

        }

        public bool GetCaminhaoEstaDisponivel(int id)
        {
            return _context.TabelaCaminhao.Find(id).CaminhaoEstaDisponivel;
        }

        public IEnumerable<CaminhaoModel> GetAll()
        {
            return _context.TabelaCaminhao
                .Include(c => c.AgendasCriadasComEsteCaminhao)
                .ToList();
        }

        public IEnumerable<CaminhaoModel> GetAll(int page, int size)
        {
            return _context.TabelaCaminhao
                .Include(c => c.AgendasCriadasComEsteCaminhao)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<CaminhaoModel> GetAllReference(int lastReference, int size)
        {
            var caminhoes = _context.TabelaCaminhao
                .Include(c => c.AgendasCriadasComEsteCaminhao)
                .Where(c => c.CaminhaoId > lastReference)
                                .OrderBy(c => c.CaminhaoId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return caminhoes;
        }
    }
}
