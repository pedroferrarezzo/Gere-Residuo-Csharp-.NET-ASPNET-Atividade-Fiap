namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class AgendaModel
    {
        
        public int AgendaId { get; set; }

        // Relacionamento com CaminhaoModel
        public int CaminhaoId { get; set; }
        public CaminhaoModel CaminhaoAlocadoParaAgenda { get; set; }
        
        // Relacionamento com MotoristaModel
        public int MotoristaId { get; set; }
        public MotoristaModel MotoristaAlocadoParaAgenda { get; set; }

        // Relacionamento com BairroModel
        public int BairroId { get; set; }
        public BairroModel BairroAgendadoParaColeta { get; set; }

        // Relacionamento com NotificacaoModel
        public NotificacaoModel NotificacaoGeradaParaEstaAgenda { get; set; }

        public DateOnly DiaColetaDeLixo { get; set; }

        public TipoResiduo TipoResiduo { get; set; }

        public StatusColetaDeLixo StatusColetaDeLixoAgendada { get; set; } = StatusColetaDeLixo.EM_ANDAMENTO;

        public long PesoColetadoDeLixoKg { get; set; } = 0;
    }
}
