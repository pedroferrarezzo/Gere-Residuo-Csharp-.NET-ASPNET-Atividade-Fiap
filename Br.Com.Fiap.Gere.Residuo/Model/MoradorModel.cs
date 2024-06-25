namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class MoradorModel
    {
        public int MoradorId { get; set; }

        // Relacionamento com BairroModel
        public int BairroId { get; set; }
        public BairroModel BairroDoMorador { get; set; }

        public string MoradorNome { get; set; }

        public string MoradorEmail { get; set; }
    }
}
