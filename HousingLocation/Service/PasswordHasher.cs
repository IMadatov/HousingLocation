using System.Security.Cryptography;
using System.Text;

namespace HousingLocation.Service
{
    public static class PasswordHasher
    {
        public static string ComputeHash(string password,string pepper,int iteration)
        {
            if(iteration < 0) return password;

            using var sha256 = SHA256.Create();
            
            var passwordPepper=password+pepper;

            var byteValue= Encoding.UTF8.GetBytes(passwordPepper);
            
            var byteHash= sha256.ComputeHash(byteValue);

            var hash= Convert
                .ToBase64String(byteHash);

            return ComputeHash(hash, pepper, iteration-1);
                
        }
    }
}
