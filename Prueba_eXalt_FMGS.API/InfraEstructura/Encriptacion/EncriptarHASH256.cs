using System.Security.Cryptography;
using System.Text;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Encriptacion
{
    public static class EncriptarHASH256
    {
        public static string Encriptar(string request)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(request);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
