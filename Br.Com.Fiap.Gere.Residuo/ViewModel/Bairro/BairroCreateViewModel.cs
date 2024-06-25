using Br.Com.Fiap.Gere.Residuo.Model;
using System.ComponentModel.DataAnnotations;

namespace Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro
{
    public class BairroCreateViewModel
    {
        [Required(ErrorMessage = $"O nome do bairro é obrigatório!")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome do bairro deve ter entre 1 e 100 caracteres.")]
        public string BairroNome { get; set; }

        [Required(ErrorMessage = $"A quantidade de lixeiras do bairro é obrigatória!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de lixeiras deve ser um número positivo maior que 0.")]
        
        public int QuantidadeLixeiras { get; set; }

        [Required(ErrorMessage = $"O peso médido de cada lixeira em KG é obrigatório!")]
        [Range(1, long.MaxValue, ErrorMessage = "O peso médio das lixeiras deve ser um número positivo maior que 0.")]
        public long PesoMedioLixeirasKg { get; set; }

    }
}
