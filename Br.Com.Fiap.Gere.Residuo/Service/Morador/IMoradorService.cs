using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Morador
{
    public interface IMoradorService
    {
        IEnumerable<MoradorModel> ListarMoradores();
        MoradorModel ObterMoradorPorId(int id);

        IEnumerable<MoradorModel> ObterMoradorPorBairroId(int bairroId);

        void CriarMorador(MoradorModel morador);
        void AtualizarMorador(MoradorModel morador);
        void DeletarMorador(int id);

        IEnumerable<MoradorModel> ListarMoradoresPaginacaoReference(int lastReference, int size);

        public IEnumerable<MoradorModel> ListarMoradoresPaginacaoPage(int page, int size);

    }
}
