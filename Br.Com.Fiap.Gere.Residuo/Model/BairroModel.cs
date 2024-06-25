namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class BairroModel
    {
        public int BairroId { get; set; }

        // Relacionamento com MoradorModel
        public List<MoradorModel> MoradoresDoBairro { get; set; }

        // Relacionamento com AgendaModel
        public List<AgendaModel> AgendasDeColetaDeLixoDoBairro { get; set; }

        public string BairroNome { get; set; }

        public int QuantidadeLixeiras { get; set; }

        public long PesoMedioLixeirasKg { get; set; }

        public int PercentualColetaLixoBairro { get; set; } = 100;

        public bool BairroEstaDisponivel { get; set; } = true;

        
    }
}
