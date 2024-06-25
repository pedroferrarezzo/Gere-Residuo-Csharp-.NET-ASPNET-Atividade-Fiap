using Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario;
using System.Security.Cryptography;
using System.Text;

namespace Br.Com.Fiap.Gere.Residuo.Utils.Hash
{
    public class HashUtils : IHashUtils
    {
        private readonly SHA256 _sha256;
        private readonly StringBuilder _builder;

        public HashUtils(SHA256 sha256, StringBuilder stringBuilder) { 
            _sha256 = sha256;
            _builder = stringBuilder;
        }

        public string gerarSha256(string texto)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
            byte[] hashBytes = _sha256.ComputeHash(inputBytes);

            for (int i = 0; i < hashBytes.Length; i++)
            {
                _builder.Append(hashBytes[i].ToString("x2"));
            }
            return _builder.ToString();
        }
    }
}
