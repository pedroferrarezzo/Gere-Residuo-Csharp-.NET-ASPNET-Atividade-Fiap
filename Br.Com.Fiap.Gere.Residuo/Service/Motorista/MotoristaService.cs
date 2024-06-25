using Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista;
using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Motorista
{
    public class MotoristaService : IMotoristaService
    {
        private readonly IMotoristaRepository _repository;

        public MotoristaService(IMotoristaRepository repository)
        {
            _repository = repository;
        }

        public void CriarMotorista(MotoristaModel motorista)
        {
            _repository.Add(motorista);
        }

        public void AtualizarMotorista(MotoristaModel motorista)
        {
            _repository.Update(motorista);
        }

        public void AtualizarMotoristaEstaDisponivel(int motoristaId, bool disponibilidade)
        {
            var motorista = _repository.GetById(motoristaId);
            if (motorista != null)
            {
                _repository.UpdateMotoristaEstaDisponivel(motoristaId, disponibilidade);
            }
        }

        public void DeletarMotorista(int id)
        {
            var motorista = _repository.GetById(id);
            if (motorista != null)
            {
                _repository.Delete(motorista);
            }
        }

        public MotoristaModel ObterMotoristaPorId(int id)
        {
            return _repository.GetById(id);
        }

        public bool ObterMotoristaEstaDisponivel(int id)
        {
            return _repository.GetMotoristaEstaDisponivel(id);
        }

        public IEnumerable<MotoristaModel> ListarMotoristas()
        {
            return _repository.GetAll();
        }

        public IEnumerable<MotoristaModel> ListarMotoristasPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<MotoristaModel> ListarMotoristasPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
