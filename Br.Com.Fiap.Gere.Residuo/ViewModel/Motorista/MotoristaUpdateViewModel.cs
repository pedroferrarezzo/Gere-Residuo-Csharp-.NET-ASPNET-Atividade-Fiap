using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Caminhao;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista
{
    public class MotoristaUpdateViewModel
    {
        [Required(ErrorMessage = $"O Id do motorista é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do motorista deve ser um número positivo maior que 0.")]
        public int MotoristaId { get; set; }

        [Required(ErrorMessage = $"O nome do motorista é obrigatório!")]
        [RegularExpression(@"^[\p{L}\p{M}\s]+$", ErrorMessage = "O nome do motorista deve conter apenas letras e espaços.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O nome do motorista deve ter entre 1 e 50 caracteres.")]
        public string MotoristaNome { get; set; }

        [Required(ErrorMessage = $"O cpf do motorista é obrigatório!")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O cpf do motorista deve estar no formato correto! (123.456.789-09)")]
        public string MotoristaCpf { get; set; }

        [Required(ErrorMessage = $"O número do motorista é obrigatório!")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O número do motorista deve estar no formato correto! (987654321)")]
        public string MotoristaNrCelular { get; set; }

        [Required(ErrorMessage = $"O DDD do motorista é obrigatório!")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "O DDD do motorista deve estar no formato correto! (11)")]
        public string MotoristaNrCelularDdd { get; set; }

        [Required(ErrorMessage = $"O DDI do motorista é obrigatório!")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessage = "O DDI do motorista deve estar no formato correto! (1, 55, 201)")]
        public string MotoristaNrCelularDdi { get; set; }

    }
}
