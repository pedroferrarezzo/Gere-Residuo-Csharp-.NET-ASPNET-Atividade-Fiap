using Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Utils.Email;
using Microsoft.IdentityModel.Tokens;

namespace Br.Com.Fiap.Gere.Residuo.Service.Notificacao
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepository _repository;
        private readonly ISmtpUtils _smtp;

        public NotificacaoService(INotificacaoRepository repository, ISmtpUtils smtp)
        {
            _repository = repository;
            _smtp = smtp;
        }

        public void CriarNotificacao(NotificacaoModel notificacao)
        {
            _repository.Add(notificacao);
        }

        public void AtualizarNotificacao(NotificacaoModel notificacao)
        {
            _repository.Update(notificacao);
        }

        public async void DisparaNotificacoes(NotificacaoModel notificacao)
        {
            var listaDeMoradoresDoBairro = notificacao.AgendaDaNotificacao.BairroAgendadoParaColeta.MoradoresDoBairro;

            if (!listaDeMoradoresDoBairro.IsNullOrEmpty())
            {
                foreach (MoradorModel morador in listaDeMoradoresDoBairro)
                {
                    await _smtp.SendEmailAsync(morador.MoradorEmail,
                        "Gere Residuo Company - Alerta de coleta de resíduos",
                        $"Olá, caro {morador.MoradorNome}!\n" +
                        $"Informamos que uma coleta de lixo foi agendada para o seu bairro na data de: {notificacao.AgendaDaNotificacao.DiaColetaDeLixo.ToString("dd-MM-yyyy")}.\n" +
                        $"Na ocasião, retiraremos do seu bairro: {notificacao.AgendaDaNotificacao.BairroAgendadoParaColeta.BairroNome} o seguindo tipo de resíduo: {notificacao.AgendaDaNotificacao.TipoResiduo}.\n" +
                        $"Apenas para lembrar, o percentual de coleta de lixo atual do seu bairro é de: {notificacao.AgendaDaNotificacao.BairroAgendadoParaColeta.PercentualColetaLixoBairro}%\n" +
                        "Se deseja melhorar esta métrica, contribua colocando seu lixo para fora nos lugares e dias adequados! O meio ambiente agradece.\n" +
                        "\n" +
                        "Att, Gere residuo Company");
                }
            }
        }

        public void DeletarNotificacao(int id)
        {
            var notificacao = _repository.GetById(id);
            if (notificacao != null)
            {
                this.DisparaNotificacoes(notificacao);
                _repository.Delete(notificacao);
            }
        }

        public NotificacaoModel ObterNotificacaoPorId(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<NotificacaoModel> ListarNotificacoes()
        {
            return _repository.GetAll();
        }

        public IEnumerable<NotificacaoModel> ListarNotificacoesPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<NotificacaoModel> ListarNotificacoesPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
