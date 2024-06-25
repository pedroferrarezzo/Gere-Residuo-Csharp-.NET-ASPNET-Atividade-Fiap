using Br.Com.Fiap.Gere.Residuo.Data.Repository.Agenda;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.Service.Notificacao;

namespace Br.Com.Fiap.Gere.Residuo.Service.Agenda
{
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _repository;
        private readonly IBairroService _bairroService;
        private readonly ICaminhaoService _caminhaoService;
        private readonly IMotoristaService _motoristaService;
        private readonly INotificacaoService _notificacaoService;

        public AgendaService(IAgendaRepository repository, INotificacaoService notificacaoService, IBairroService bairroService, ICaminhaoService caminhaoService, IMotoristaService motoristaService)
        {
            _repository = repository;
            _bairroService = bairroService;
            _caminhaoService = caminhaoService;
            _motoristaService = motoristaService;
            _notificacaoService = notificacaoService;
        }

        public void CriarAgenda(AgendaModel agenda)
        {
            var bairroFromBodyEstaDisponivel = _bairroService.ObterBairroEstaDisponivel(agenda.BairroId);
            var motoristaFromBodyEstaDisponivel = _motoristaService.ObterMotoristaEstaDisponivel(agenda.MotoristaId);
            var caminhaoFromBodyEstaDisponivel = _caminhaoService.ObterCaminhaoEstaDisponivel(agenda.CaminhaoId);

            if (bairroFromBodyEstaDisponivel == false)
            {
                throw new Exception.BairroNaoEstaDisponivelException();
            }
            if (motoristaFromBodyEstaDisponivel == false)
            {
                throw new Exception.MotoristaNaoEstaDisponivelException();
            }

            if (caminhaoFromBodyEstaDisponivel == false)
            {
                throw new Exception.CaminhaoNaoEstaDisponivelException();
            }

            var notificacao = new Model.NotificacaoModel();
            _repository.Add(agenda);
            notificacao.AgendaId = agenda.AgendaId;
            _notificacaoService.CriarNotificacao(notificacao);

            _bairroService.AtualizarBairroEstaDisponivel(agenda.BairroId, false);
            _caminhaoService.AtualizarCaminhaoEstaDisponivel(agenda.CaminhaoId, false);
            _motoristaService.AtualizarMotoristaEstaDisponivel(agenda.MotoristaId, false);
            
        }

        public void AtualizarAgenda(AgendaModel agenda, BairroModel bairroAtualDaAgenda, MotoristaModel motoristaAtualDaAgenda, CaminhaoModel caminhaoAtualDaAgenda, StatusColetaDeLixo statusColetaDeLixoAtualDaAgenda)
        {
            var bairroFuturoDaAgenda = _bairroService.ObterBairroPorId(agenda.BairroId);
            var motoristaFuturoDaAgenda = _motoristaService.ObterMotoristaPorId(agenda.MotoristaId);
            var caminhaoFuturoDaAgenda = _caminhaoService.ObterCaminhaoPorId(agenda.CaminhaoId);
            var notificacaoDaAgenda = _repository.GetById(agenda.AgendaId).NotificacaoGeradaParaEstaAgenda;

            if (agenda.BairroId != bairroAtualDaAgenda.BairroId)
            {
                if (bairroFuturoDaAgenda.BairroEstaDisponivel == false)
                {
                    throw new Exception.BairroNaoEstaDisponivelException();
                }
            }

            if (agenda.MotoristaId != motoristaAtualDaAgenda.MotoristaId)
            {
                if (motoristaFuturoDaAgenda.MotoristaEstaDisponivel == false)
                {
                    throw new Exception.MotoristaNaoEstaDisponivelException();
                }
            }

            if (agenda.CaminhaoId != caminhaoAtualDaAgenda.CaminhaoId)
            {
                if (caminhaoFuturoDaAgenda.CaminhaoEstaDisponivel == false)
                {
                    throw new Exception.CaminhaoNaoEstaDisponivelException();
                }
            }

            if (statusColetaDeLixoAtualDaAgenda == StatusColetaDeLixo.FINALIZADA)
            {
                throw new Exception.AgendaJaEstaFinalizadaException();
            }

            if (agenda.StatusColetaDeLixoAgendada == StatusColetaDeLixo.EM_ANDAMENTO)
            {
                if (agenda.BairroId != bairroAtualDaAgenda.BairroId)
                {
                    _bairroService.AtualizarBairroEstaDisponivel(bairroAtualDaAgenda.BairroId, true);
                    _bairroService.AtualizarBairroEstaDisponivel(bairroFuturoDaAgenda.BairroId, false);
                }

                if (agenda.MotoristaId != motoristaAtualDaAgenda.MotoristaId)
                {
                    _motoristaService.AtualizarMotoristaEstaDisponivel(motoristaAtualDaAgenda.MotoristaId, true);
                    _motoristaService.AtualizarMotoristaEstaDisponivel(motoristaFuturoDaAgenda.MotoristaId, false);
                }

                if (agenda.CaminhaoId != caminhaoAtualDaAgenda.CaminhaoId)
                {
                    _caminhaoService.AtualizarCaminhaoEstaDisponivel(caminhaoAtualDaAgenda.CaminhaoId, true);
                    _caminhaoService.AtualizarCaminhaoEstaDisponivel(caminhaoFuturoDaAgenda.CaminhaoId, false);
                }
            }

            else if (agenda.StatusColetaDeLixoAgendada == StatusColetaDeLixo.FINALIZADA)
            {
                if (notificacaoDaAgenda != null)
                {
                    throw new Exception.AgendaPossuiNotificacoesNaoDisparadasException($"{notificacaoDaAgenda.NotificacaoId}");
                }

                _bairroService.AtualizarBairroEstaDisponivel(bairroFuturoDaAgenda.BairroId, true);

                _motoristaService.AtualizarMotoristaEstaDisponivel(motoristaFuturoDaAgenda.MotoristaId, true);

                _caminhaoService.AtualizarCaminhaoEstaDisponivel(caminhaoFuturoDaAgenda.CaminhaoId, true);

                var pesoTotalDeLixo = bairroFuturoDaAgenda.PesoMedioLixeirasKg * bairroFuturoDaAgenda.QuantidadeLixeiras;
                var percentualColetaDeLixoDoBairro = (double)agenda.PesoColetadoDeLixoKg / (double)pesoTotalDeLixo * 100.0;
                _bairroService.AtualizarPercentualColetaLixoBairro(bairroFuturoDaAgenda.BairroId, (int)percentualColetaDeLixoDoBairro);
                    
            }

            _repository.Update(agenda);
            
            
        }

        public void DeletarAgenda(int id)
        {
            var agenda = _repository.GetById(id);
            if (agenda != null)
            {
                _repository.Delete(agenda);
                _bairroService.AtualizarBairroEstaDisponivel(agenda.BairroId, true);
                _caminhaoService.AtualizarCaminhaoEstaDisponivel(agenda.CaminhaoId, true);
                _motoristaService.AtualizarMotoristaEstaDisponivel(agenda.MotoristaId, true);
            }
        }

        public AgendaModel ObterAgendaPorId(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<AgendaModel> ListarAgendas()
        {
            return _repository.GetAll();
        }

        public IEnumerable<AgendaModel> ListarAgendasPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<AgendaModel> ListarAgendasPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
