using Br.Com.Fiap.Gere.Residuo.Model;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario
{
    public class UsuarioLoginViewModel
    {

        [Required(ErrorMessage = $"O email do morador é obrigatório!")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string UsuarioEmail { get; set; }

        [Required(ErrorMessage = $"A senha do usuário é obrigatória!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{8,}$", ErrorMessage = "A senha do usuário deve conter no mínimo oito caractéres e pelo menos uma letra maiúsucla, uma minúscula, um número e um caractére especial.")]
        public string UsuarioSenha { get; set; }

    }
}
