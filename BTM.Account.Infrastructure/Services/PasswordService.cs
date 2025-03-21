using BTM.Account.Application.Abstractions;
using System.Security.Cryptography;

namespace BTM.Account.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16; // 16 bytes = 128 bits
        private const int HashSize = 20; // 20 bytes = 160 bits (recommended size)
        private const int Iterations = 10000; // Number of iterations (can be tuned for security/performance)

        // Hash a password using PBKDF2 with a random salt
        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            // Generate the hash using PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Combine the salt and hash to store them together
                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Return the combined result as a Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Verify the entered password matches the stored hash
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Decode the Base64 string to get the hash bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt (first 16 bytes)
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Extract the hash (remaining bytes)
            byte[] storedPasswordHash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, HashSize);

            // Generate the hash of the entered password using the same salt and number of iterations
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations))
            {
                byte[] enteredPasswordHash = pbkdf2.GetBytes(HashSize);

                // Compare the entered hash with the stored hash byte-by-byte
                for (int i = 0; i < HashSize; i++)
                {
                    if (enteredPasswordHash[i] != storedPasswordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true; // Passwords match
        }
    }
}
