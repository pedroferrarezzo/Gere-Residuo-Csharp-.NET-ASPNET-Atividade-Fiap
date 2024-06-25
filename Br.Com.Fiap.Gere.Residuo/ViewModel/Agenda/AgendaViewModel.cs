using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda
{
    public class AgendaViewModel
    {
        public int AgendaId { get; set; }
        // Relacionamento com CaminhaoModel
        public CaminhaoViewModelForResponse CaminhaoAlocadoParaAgenda { get; set; }

        // Relacionamento com MotoristaModel
        public MotoristaViewModelForResponse MotoristaAlocadoParaAgenda { get; set; }

        // Relacionamento com BairroModel
        public BairroViewModelForResponse BairroAgendadoParaColeta { get; set; }

        // Relacionamento com NotificacaoModel
        public NotificacaoViewModelForResponse NotificacaoGeradaParaEstaAgenda { get; set; }

        public DateOnly DiaColetaDeLixo { get; set; }

        public TipoResiduo TipoResiduo { get; set; }

        public StatusColetaDeLixo StatusColetaDeLixoAgendada { get; set; }

        public long PesoColetadoDeLixoKg { get; set; }
    }
}
