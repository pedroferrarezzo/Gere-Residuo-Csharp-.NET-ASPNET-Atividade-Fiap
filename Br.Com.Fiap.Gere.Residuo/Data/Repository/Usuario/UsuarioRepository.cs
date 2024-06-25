using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Model;
using Microsoft.EntityFrameworkCore;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DatabaseContext _context;

        public UsuarioRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(UsuarioModel usuario)
        {
            _context.TabelaUsuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Delete(UsuarioModel usuario)
        {
            _context.TabelaUsuario.Remove(usuario);
            _context.SaveChanges();
        }

        public void Update(UsuarioModel usuario)
        {
            _context.TabelaUsuario.Update(usuario);
            _context.SaveChanges();
        }

        public UsuarioModel GetById(int id)
        {
            return _context.TabelaUsuario.Find(id);

        }

        public IEnumerable<UsuarioModel> GetAll()
        {
            return _context.TabelaUsuario.ToList();
        }

        public IEnumerable<UsuarioModel> GetAll(int page, int size)
        {
            return _context.TabelaUsuario
                .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<UsuarioModel> GetAllReference(int lastReference, int size)
        {
            var usuarios = _context.TabelaUsuario
                .Where(u => u.UsuarioId > lastReference)
                                .OrderBy(u => u.UsuarioId)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();
            return usuarios;
        }
    }
}
