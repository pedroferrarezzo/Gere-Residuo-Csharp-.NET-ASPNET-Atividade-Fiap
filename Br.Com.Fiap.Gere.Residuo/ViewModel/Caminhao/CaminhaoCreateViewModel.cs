using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Caminhao;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao
{
    public class CaminhaoCreateViewModel
    {
       
        [Required(ErrorMessage = $"A placa do caminhão é obrigatória!")]
        [RegularExpression(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$", ErrorMessage = "A placa do caminhão deve estar no formato Mercosul - Ex: ABC1D34")]
        public string CaminhaoPlaca { get; set; }

        [Required(ErrorMessage = $"A data de fabricação do caminhão é obrigatória!")]
        [DataType(DataType.Date, ErrorMessage = "A data de fabricação do caminhão deve estar no formato correto!")]
        [DataFabricacaoCreateCaminhao]
        public DateOnly DataFabricacao { get; set; }

        [Required(ErrorMessage = $"A marca do caminhão é obrigatória!")]
        [RegularExpression(@"^[\p{L}\p{M}\s]+$", ErrorMessage = "A marca do caminhão deve conter apenas letras e espaços.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "A marca do caminhão deve ter entre 1 e 50 caracteres.")]
        public string CaminhaoMarca { get; set; }

        [Required(ErrorMessage = $"O modelo do caminhão é obrigatório!")]
        [RegularExpression(@"^[\p{L}\p{M}\s]+$", ErrorMessage = "O modelo do caminhão deve conter apenas letras e espaços.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "O modelo do caminhão deve ter entre 1 e 50 caracteres.")]
        public string CaminhaoModelo { get; set; }

    }
}
