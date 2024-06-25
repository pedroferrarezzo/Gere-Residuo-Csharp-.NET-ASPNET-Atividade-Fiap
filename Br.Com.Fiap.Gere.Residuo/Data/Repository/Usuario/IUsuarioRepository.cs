using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario
{
    public interface IUsuarioRepository
    {
        IEnumerable<UsuarioModel> GetAll();
        UsuarioModel GetById(int id);
        void Add(UsuarioModel usuario);
        void Update(UsuarioModel usuario);
        void Delete(UsuarioModel usuario);

        // Paginacao por Skip
        IEnumerable<UsuarioModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<UsuarioModel> GetAllReference(int lastReference, int size);
    }
}
