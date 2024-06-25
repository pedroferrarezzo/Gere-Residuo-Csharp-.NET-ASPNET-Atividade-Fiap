using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Bairro
{
    public interface IBairroService
    {
        IEnumerable<BairroModel> ListarBairros();
        BairroModel ObterBairroPorId(int id);
        bool ObterBairroEstaDisponivel(int id);
        void CriarBairro(BairroModel bairro);
        void AtualizarBairro(BairroModel bairro);
        void AtualizarBairroEstaDisponivel(int bairroId, bool disponibilidade);
        void AtualizarPercentualColetaLixoBairro(int bairroId, int percentualColetaLixoBairro);
        void DeletarBairro(int id);

        IEnumerable<BairroModel> ListarBairrosPaginacaoReference(int lastReference, int size);

        IEnumerable<BairroModel> ListarBairrosPaginacaoPage(int page, int size);

    }
}
