namespace Br.Com.Fiap.Gere.Residuo.Model
{
    public class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; }

        public string UsuarioEmail { get; set; }

        public string UsuarioSenha { get; set; }

        public UsuarioRole UsuarioRole { get; set; }
    }
}
