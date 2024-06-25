using Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Caminhao
{
    public class CaminhaoService : ICaminhaoService
    {
        private readonly ICaminhaoRepository _repository;

        public CaminhaoService(ICaminhaoRepository repository)
        {
            _repository = repository;
        }

        public void CriarCaminhao(CaminhaoModel caminhao)
        {
            _repository.Add(caminhao);
        }

        public void AtualizarCaminhao(CaminhaoModel caminhao)
        {
            _repository.Update(caminhao);
        }

        public void AtualizarCaminhaoEstaDisponivel(int caminhaoId, bool disponibilidade)
        {
            var caminhao = _repository.GetById(caminhaoId);
            if (caminhao != null)
            {
                _repository.UpdateCaminhaoEstaDisponivel(caminhaoId, disponibilidade);
            }
        }

        public void DeletarCaminhao(int id)
        {
            var caminhao = _repository.GetById(id);
            if (caminhao != null)
            {
                _repository.Delete(caminhao);
            }
        }

        public CaminhaoModel ObterCaminhaoPorId(int id)
        {
            return _repository.GetById(id);
        }

        public bool ObterCaminhaoEstaDisponivel(int id)
        {
            return _repository.GetCaminhaoEstaDisponivel(id);
        }

        public IEnumerable<CaminhaoModel> ListarCaminhoes()
        {
            return _repository.GetAll();
        }

        public IEnumerable<CaminhaoModel> ListarCaminhoesPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<CaminhaoModel> ListarCaminhoesPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
