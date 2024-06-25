using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda
{
    public class AgendaViewModelForResponse
    {
        public int AgendaId { get; set; }

        // Relacionamento com CaminhaoModel
        public int CaminhaoId { get; set; }

        // Relacionamento com MotoristaModel
        public int MotoristaId { get; set; }

        // Relacionamento com BairroModel
        public int BairroId { get; set; }

        public DateOnly DiaColetaDeLixo { get; set; }

        public TipoResiduo TipoResiduo { get; set; }

        public StatusColetaDeLixo StatusColetaDeLixoAgendada { get; set; }

        public long PesoColetadoDeLixoKg { get; set; }
    }
}
