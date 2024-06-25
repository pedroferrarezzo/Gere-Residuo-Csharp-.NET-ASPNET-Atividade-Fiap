using Br.Com.Fiap.Gere.Residuo.Model;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Morador
{
    public class MoradorCreateViewModel
    {
        // Relacionamento com BairroModel
        [Required(ErrorMessage = $"O Id do bairro do morador é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do bairro do morador deve ser um número positivo maior que 0.")]
        public int BairroId { get; set; }

        [Required(ErrorMessage = $"O nome do morador é obrigatório!")]
        [RegularExpression(@"^[\p{L}\p{M}\s]+$", ErrorMessage = "O nome do morador deve conter apenas letras e espaços.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O nome do morador deve ter entre 1 e 50 caracteres.")]
        public string MoradorNome { get; set; }

        [Required(ErrorMessage = $"O email do morador é obrigatório!")]
        [EmailAddress(ErrorMessage = "Insira um endereço de e-mail válido.")]
        public string MoradorEmail { get; set; }

    }
}
