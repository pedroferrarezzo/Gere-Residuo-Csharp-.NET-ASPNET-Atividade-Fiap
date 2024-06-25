using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Morador
{
    public interface IMoradorRepository
    {
        IEnumerable<MoradorModel> GetAll();
        MoradorModel GetById(int id);
        IEnumerable<MoradorModel> GetByBairroId(int bairroId);
        void Add(MoradorModel morador);
        void Update(MoradorModel morador);
        void Delete(MoradorModel morador);

        // Paginacao por Skip
        IEnumerable<MoradorModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<MoradorModel> GetAllReference(int lastReference, int size);
    }
}
