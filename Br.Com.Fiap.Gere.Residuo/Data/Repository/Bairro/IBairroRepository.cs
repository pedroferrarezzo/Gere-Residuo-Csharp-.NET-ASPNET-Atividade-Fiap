using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro
{
    public interface IBairroRepository
    {
        IEnumerable<BairroModel> GetAll();
        BairroModel GetById(int id);
        bool GetBairroEstaDisponivel(int id);
        void Add(BairroModel bairro);
        void Update(BairroModel bairro);
        void UpdateBairroEstaDisponivel(int bairroId, bool disponibilidade);
        void UpdatePercentualColetaLixoBairro(int bairroId, int percentualColetaLixoBairro);

        void Delete(BairroModel bairro);

        // Paginacao por Skip
        IEnumerable<BairroModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<BairroModel> GetAllReference(int lastReference, int size);
    }
}
