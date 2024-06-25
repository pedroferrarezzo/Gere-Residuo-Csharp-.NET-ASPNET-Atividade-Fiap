using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Caminhao
{
    public interface ICaminhaoService
    {
        IEnumerable<CaminhaoModel> ListarCaminhoes();
        CaminhaoModel ObterCaminhaoPorId(int id);
        bool ObterCaminhaoEstaDisponivel(int id);
        void CriarCaminhao(CaminhaoModel caminhao);
        void AtualizarCaminhao(CaminhaoModel caminhao);
        void AtualizarCaminhaoEstaDisponivel(int caminhaoId, bool disponibilidade);
        void DeletarCaminhao(int id);

        IEnumerable<CaminhaoModel> ListarCaminhoesPaginacaoReference(int lastReference, int size);

        public IEnumerable<CaminhaoModel> ListarCaminhoesPaginacaoPage(int page, int size);

    }
}
