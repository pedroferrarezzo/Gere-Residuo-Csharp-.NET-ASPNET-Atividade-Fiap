using Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Utils.Hash;

namespace Br.Com.Fiap.Gere.Residuo.Service.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IHashUtils _hashUtils;

        public UsuarioService(IUsuarioRepository repository, IHashUtils hashUtils)
        {
            _repository = repository;
            _hashUtils = hashUtils;
        }


        public UsuarioModel Autenticar(string email, string senha)
        {
            var senhaHash = _hashUtils.gerarSha256(senha);
            var usuarios = this.ListarUsuarios();

            return usuarios.FirstOrDefault(u => u.UsuarioEmail == email && u.UsuarioSenha == senhaHash);

        }


        public void CriarUsuario(UsuarioModel usuario)
        {
            usuario.UsuarioSenha = _hashUtils.gerarSha256(usuario.UsuarioSenha);
            _repository.Add(usuario);
        }

        public void AtualizarUsuario(UsuarioModel usuario)
        {
            usuario.UsuarioSenha = _hashUtils.gerarSha256(usuario.UsuarioSenha);
            _repository.Update(usuario);
        }

        public void DeletarUsuario(int id)
        {
            var usuario = _repository.GetById(id);
            if (usuario != null)
            {
                _repository.Delete(usuario);
            }
        }

        public UsuarioModel ObterUsuarioPorId(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<UsuarioModel> ListarUsuarios()
        {
            return _repository.GetAll();
        }


        public IEnumerable<UsuarioModel> ListarUsuariosPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<UsuarioModel> ListarUsuariosPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
