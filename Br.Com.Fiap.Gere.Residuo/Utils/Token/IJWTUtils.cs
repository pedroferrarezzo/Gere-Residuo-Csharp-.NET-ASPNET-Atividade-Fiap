using Br.Com.Fiap.Gere.Residuo.Model;

namespace Br.Com.Fiap.Gere.Residuo.Utils.Token
{
    public interface IJWTUtils
    {
        public string GenerateJwtToken(UsuarioModel user, double minutesToExpire);
    }
}
