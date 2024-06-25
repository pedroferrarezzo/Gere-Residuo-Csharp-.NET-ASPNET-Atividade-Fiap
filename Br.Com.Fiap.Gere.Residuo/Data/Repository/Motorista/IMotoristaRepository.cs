using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista
{
    public interface IMotoristaRepository
    {
        IEnumerable<MotoristaModel> GetAll();
        MotoristaModel GetById(int id);
        bool GetMotoristaEstaDisponivel(int id);
        void Add(MotoristaModel motorista);
        void Update(MotoristaModel motorista);
        void UpdateMotoristaEstaDisponivel(int motoristaId, bool disponibilidade);
        void Delete(MotoristaModel motorista);

        // Paginacao por Skip
        IEnumerable<MotoristaModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<MotoristaModel> GetAllReference(int lastReference, int size);
    }
}
