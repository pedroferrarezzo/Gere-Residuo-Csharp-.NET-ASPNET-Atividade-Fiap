using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista
{
    public class MotoristaRepository : IMotoristaRepository
    {
        private readonly DatabaseContext _context;

        public MotoristaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(MotoristaModel motorista)
        {
            _context.TabelaMotorista.Add(motorista);
            _context.SaveChanges();
        }

        public void Delete(MotoristaModel motorista)
        {
            _context.TabelaMotorista.Remove(motorista);
            _context.SaveChanges();
        }

        public void Update(MotoristaModel motorista)
        {
            _context.TabelaMotorista.Update(motorista);
            _context.SaveChanges();
        }

        public void UpdateMotoristaEstaDisponivel(int motoristaId, bool disponibilidade)
        {
            var motorista = this.GetById(motoristaId);
            motorista.MotoristaEstaDisponivel = disponibilidade;
            _context.SaveChanges();
        }

        public bool GetMotoristaEstaDisponivel(int id)
        {
            return _context.TabelaMotorista.Find(id).MotoristaEstaDisponivel;
        }


        public MotoristaModel GetById(int id)
        {
            return _context.TabelaMotorista
                .Include(m => m.AgendasCriadasComEsteMotorista)
                .FirstOrDefault(m => m.MotoristaId == id);

        }

        public IEnumerable<MotoristaModel> GetAll()
        {
            return _context.TabelaMotorista
                .Include(m => m.AgendasCriadasComEsteMotorista)
                .ToList();
        }

        public IEnumerable<MotoristaModel> GetAll(int page, int size)
        {
            return _context.TabelaMotorista
                .Include(m => m.AgendasCriadasComEsteMotorista)
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<MotoristaModel> GetAllReference(int lastReference, int size)
        {
            var motoristas = _context.TabelaMotorista
                .Include(m => m.AgendasCriadasComEsteMotorista)
                .Where(m => m.MotoristaId > lastReference)
                                .OrderBy(m => m.MotoristaId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return motoristas;
        }
    }
}
