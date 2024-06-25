using Br.Com.Fiap.Gere.Residuo.Data.Repository.Morador;
using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Morador
{
    public class MoradorService : IMoradorService
    {
        private readonly IMoradorRepository _repository;

        public MoradorService(IMoradorRepository repository)
        {
            _repository = repository;
        }

        public void CriarMorador(MoradorModel morador)
        {
            _repository.Add(morador);
        }

        public void AtualizarMorador(MoradorModel morador)
        {
            _repository.Update(morador);
        }

        public void DeletarMorador(int id)
        {
            var morador = _repository.GetById(id);
            if (morador != null)
            {
                _repository.Delete(morador);
            }
        }

        public MoradorModel ObterMoradorPorId(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<MoradorModel> ObterMoradorPorBairroId(int bairroId)
        {
            return _repository.GetByBairroId(bairroId);
        }

        public IEnumerable<MoradorModel> ListarMoradores()
        {
            return _repository.GetAll();
        }

        public IEnumerable<MoradorModel> ListarMoradoresPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<MoradorModel> ListarMoradoresPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
