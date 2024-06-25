using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Caminhao
{
    public class DataFabricacaoCreateCaminhaoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var caminhao = (CaminhaoCreateViewModel)validationContext.ObjectInstance;
            if (caminhao.DataFabricacao > DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult($"A data de fabricação do caminhão inserida é maior que o dia de hoje: {DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")}");
            }

            return ValidationResult.Success;
        }

    }
}
