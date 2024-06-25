using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario
{
    public class UsuarioViewModel
    {
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }

        public string UsuarioEmail { get; set; }

        public UsuarioRole UsuarioRole { get; set; }

    }
}
