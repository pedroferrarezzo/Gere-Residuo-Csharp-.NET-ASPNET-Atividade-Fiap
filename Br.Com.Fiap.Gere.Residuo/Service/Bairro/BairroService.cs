using Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro;
using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Service.Bairro
{
    public class BairroService : IBairroService
    {
        private readonly IBairroRepository _repository;

        public BairroService(IBairroRepository repository)
        {
            _repository = repository;
        }

        public void CriarBairro(BairroModel bairro)
        {
            _repository.Add(bairro);
        }

        public void AtualizarBairro(BairroModel bairro)
        {
            _repository.Update(bairro);
        }

        public void AtualizarBairroEstaDisponivel(int bairroId, bool disponibilidade)
        {
            var bairro = _repository.GetById(bairroId);
            if (bairro != null)
            {
                _repository.UpdateBairroEstaDisponivel(bairroId, disponibilidade);
            }
        }

        public void AtualizarPercentualColetaLixoBairro(int bairroId, int percentualColetaLixoBairro)
        {
            var bairro = _repository.GetById(bairroId);
            if (bairro != null)
            {
                _repository.UpdatePercentualColetaLixoBairro(bairroId, percentualColetaLixoBairro);
            }
        }


        public void DeletarBairro(int id)
        {
            var bairro = _repository.GetById(id);
            if (bairro != null)
            {
                _repository.Delete(bairro);
            }
        }

        public BairroModel ObterBairroPorId(int id)
        {
            return _repository.GetById(id);
        }

        public bool ObterBairroEstaDisponivel(int id)
        {
            return _repository.GetBairroEstaDisponivel(id);
        }


        public IEnumerable<BairroModel> ListarBairros()
        {
            return _repository.GetAll();
        }

        public IEnumerable<BairroModel> ListarBairrosPaginacaoPage(int page, int size)
        {
            return _repository.GetAll(page, size);
        }

        public IEnumerable<BairroModel> ListarBairrosPaginacaoReference(int lastReference, int size)
        {
            return _repository.GetAllReference(lastReference, size);
        }

    }
}
