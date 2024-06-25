using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao
{
    public interface ICaminhaoRepository
    {
        IEnumerable<CaminhaoModel> GetAll();
        CaminhaoModel GetById(int id);
        bool GetCaminhaoEstaDisponivel(int id);
        void Add(CaminhaoModel caminhao);
        void Update(CaminhaoModel caminhao);
        void UpdateCaminhaoEstaDisponivel(int caminhaoId, bool disponibilidade);
        void Delete(CaminhaoModel caminhao);

        // Paginacao por Skip
        IEnumerable<CaminhaoModel> GetAll(int page, int size);

        // Paginacao por referencia
        IEnumerable<CaminhaoModel> GetAllReference(int lastReference, int size);
    }
}
