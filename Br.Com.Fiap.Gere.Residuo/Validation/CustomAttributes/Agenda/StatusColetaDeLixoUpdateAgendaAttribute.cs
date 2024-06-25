using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Agenda
{
    public class StatusColetaDeLixoUpdateAgendaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var agenda = (AgendaUpdateViewModel)validationContext.ObjectInstance;

            if (agenda.StatusColetaDeLixoAgendada == StatusColetaDeLixo.FINALIZADA && agenda.PesoColetadoDeLixoKg == null)
            {
                return new ValidationResult("Para finalizar a agenda você deve informar o peso da coleta de lixo em KG! - Atributo: pesoColetadoDeLixoKg");
            }
            else if (agenda.StatusColetaDeLixoAgendada == StatusColetaDeLixo.FINALIZADA && agenda.PesoColetadoDeLixoKg < 1)
            {
                return new ValidationResult("Para finalizar a agenda o atributo: pesoColetadoDeLixoKg deve ser maior que 0!");
            }

            return ValidationResult.Success;

        }
    }

}
