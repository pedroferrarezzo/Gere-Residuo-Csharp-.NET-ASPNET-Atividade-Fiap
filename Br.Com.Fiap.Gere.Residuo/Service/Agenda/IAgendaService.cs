using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Agenda
{
    public interface IAgendaService
    {
        IEnumerable<AgendaModel> ListarAgendas();
        AgendaModel ObterAgendaPorId(int id);
        void CriarAgenda(AgendaModel agenda);
        void AtualizarAgenda(AgendaModel agenda, BairroModel bairroAtualDaAgenda, MotoristaModel motoristaAtualDaAgenda, CaminhaoModel caminhaoAtualDaAgenda, StatusColetaDeLixo statusColetaDeLixoAtualDaAgenda);
        void DeletarAgenda(int id);

        IEnumerable<AgendaModel> ListarAgendasPaginacaoReference(int lastReference, int size);

        public IEnumerable<AgendaModel> ListarAgendasPaginacaoPage(int page, int size);
    }
}
