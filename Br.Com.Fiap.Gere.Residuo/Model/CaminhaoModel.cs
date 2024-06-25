namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class CaminhaoModel
    {
        public int CaminhaoId { get; set; }

        // Relacionamento com AgendaModel
        public List<AgendaModel> AgendasCriadasComEsteCaminhao { get; set; }

        public string CaminhaoPlaca { get; set; }
        public DateOnly DataFabricacao { get; set; }

        public string CaminhaoMarca { get; set; }

        public string CaminhaoModelo { get; set; }

        public bool CaminhaoEstaDisponivel { get; set; } = true;


    }
}
