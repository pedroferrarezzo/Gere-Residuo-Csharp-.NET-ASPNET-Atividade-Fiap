namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class MotoristaModel
    {
        public int MotoristaId { get; set; }

        // Relacionamento com AgendaModel
        public List<AgendaModel> AgendasCriadasComEsteMotorista { get; set; }

        public string MotoristaNome { get; set; }

        public string MotoristaCpf { get; set; }

        public string MotoristaNrCelular { get; set; }

        public string MotoristaNrCelularDdd { get; set; }

        public string MotoristaNrCelularDdi { get; set; }

        public bool MotoristaEstaDisponivel { get; set; } = true;
    }
}
