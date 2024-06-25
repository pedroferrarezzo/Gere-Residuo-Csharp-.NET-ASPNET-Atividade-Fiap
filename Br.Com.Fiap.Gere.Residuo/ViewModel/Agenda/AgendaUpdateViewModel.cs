using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Validation.CustomAttributes.Agenda;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda
{
    public class AgendaUpdateViewModel
    {
        [Required(ErrorMessage = $"O Id da agenda é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id da agenda deve ser um número positivo maior que 0.")]
        public int AgendaId { get; set; }

        [Required(ErrorMessage = $"O Id do caminhão é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do caminhão deve ser um número positivo maior que 0.")]
        public int CaminhaoId { get; set; }

        [Required(ErrorMessage = $"O Id do motorista é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do motorista deve ser um número positivo maior que 0.")]
        public int MotoristaId { get; set; }

        [Required(ErrorMessage = $"O Id do bairro é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O Id do bairro deve ser um número positivo maior que 0.")]
        public int BairroId { get; set; }

        [Required(ErrorMessage = $"A data de coleta de lixo é obrigatória!")]
        [DataType(DataType.Date, ErrorMessage = "A data de coleta de lixo deve estar no formato correto!")]
        [DiaColetaDeLixoUpdateAgenda]
        public DateOnly DiaColetaDeLixo { get; set; }


        [Required(ErrorMessage = $"O tipo de resíduo da coleta de lixo é obrigatória!")]
        public TipoResiduo TipoResiduo { get; set; }


        [Required(ErrorMessage = $"O status da coleta de lixo é obrigatório!")]
        [StatusColetaDeLixoUpdateAgenda]
        public StatusColetaDeLixo StatusColetaDeLixoAgendada { get; set; }

        [Required(ErrorMessage = $"O peso coletado de lixo é obrigatório!")]
        public long PesoColetadoDeLixoKg { get; set; }


    }
}
