using System.Security.Cryptography;
using System.Text;


//* Convertendo a senha em hash
namespace SistemLogin.Services
{
    public static class HashService
    {
        public static byte[] GerarHashBytes(string senha)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }
    }
}