using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Motorista
{
    public interface IMotoristaService
    {
        IEnumerable<MotoristaModel> ListarMotoristas();
        MotoristaModel ObterMotoristaPorId(int id);
        bool ObterMotoristaEstaDisponivel(int id);
        void CriarMotorista(MotoristaModel motorista);
        void AtualizarMotorista(MotoristaModel motorista);
        void AtualizarMotoristaEstaDisponivel(int motoristaId, bool disponibilidade);
        void DeletarMotorista(int id);

        IEnumerable<MotoristaModel> ListarMotoristasPaginacaoReference(int lastReference, int size);

        public IEnumerable<MotoristaModel> ListarMotoristasPaginacaoPage(int page, int size);

    }
}
