using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Usuario
{
    public interface IUsuarioService
    {
        IEnumerable<UsuarioModel> ListarUsuarios();
        UsuarioModel ObterUsuarioPorId(int id);
        void CriarUsuario(UsuarioModel usuario);
        void AtualizarUsuario(UsuarioModel usuario);
        void DeletarUsuario(int id);

        IEnumerable<UsuarioModel> ListarUsuariosPaginacaoReference(int lastReference, int size);

        public IEnumerable<UsuarioModel> ListarUsuariosPaginacaoPage(int page, int size);

        UsuarioModel Autenticar(string email, string senha);

    }
}
