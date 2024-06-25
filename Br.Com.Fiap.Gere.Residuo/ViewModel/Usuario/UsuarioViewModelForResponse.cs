using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario
{
    public class UsuarioViewModelForResponse
    {
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }

        public string UsuarioEmail { get; set; }

        public UsuarioRole UsuarioRole { get; set; }

    }
}
