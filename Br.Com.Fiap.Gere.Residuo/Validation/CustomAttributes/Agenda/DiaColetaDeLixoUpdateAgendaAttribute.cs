using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Agenda
{
    public class DiaColetaDeLixoUpdateAgendaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var agenda = (AgendaUpdateViewModel)validationContext.ObjectInstance;
            if (agenda.DiaColetaDeLixo < DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult($"A data de agenda da coleta de lixo é menor que o dia de hoje: {DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")}");
            }

            return ValidationResult.Success;
        }

    }
}
