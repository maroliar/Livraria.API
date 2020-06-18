using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Livraria.Service
{
    /// <summary>
    /// Classe utilizada para configurar a forma como o conteúdo do TOKEN será encriptado.
    /// </summary>
    public class LoginConfiguration
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public LoginConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
