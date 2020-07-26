using System.Linq;
using System.Text;

namespace JWTPolicyBasedAuthorization.Infrastructure
{
    public class CommonUtility
    {
        public (byte[], byte[]) CreatePasswordHash(string password)
        {
            byte[] passwordSalt;
            byte[] passwordHash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return (passwordHash,passwordSalt);
        }

        public bool ValidatePassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                    if(computedHash[i] != passwordHash[i]) return false;
            }
            return true;
        }
    }
}