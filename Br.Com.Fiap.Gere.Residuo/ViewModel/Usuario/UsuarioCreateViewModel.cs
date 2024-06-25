using Br.Com.Fiap.Gere.Residuo.Model;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = $"O nome do usuário é obrigatório!")]
        [RegularExpression(@"^[\p{L}\p{M}\s]+$", ErrorMessage = "O nome do usuário deve conter apenas letras e espaços.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O nome do usuário deve ter entre 1 e 50 caracteres.")]
        public string UsuarioNome { get; set; }

        [Required(ErrorMessage = $"O email do morador é obrigatório!")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string UsuarioEmail { get; set; }

        [Required(ErrorMessage = $"A senha do usuário é obrigatória!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{8,}$", ErrorMessage = "A senha do usuário deve conter no mínimo oito caractéres e pelo menos uma letra maiúsucla, uma minúscula, um número e um caractére especial.")]
        public string UsuarioSenha { get; set; }

        [Required(ErrorMessage = $"A Role do usuário é obrigatória!")]
        public UsuarioRole UsuarioRole { get; set; }

    }
}
