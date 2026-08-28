using FrutNatura.Core.Abstractions.Security;
using System.Security.Cryptography;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;


namespace FrutNatura.Infra.Security.Hashing
{


    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iter = 100_000;
        public string HashPassword(string password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: Iter, numBytesRequested: KeySize);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) return false;
            if (providedPassword is null) return false;

                var parts = hashedPassword.Split(".", 2);
                      
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);
            var actualHash = KeyDerivation.Pbkdf2(providedPassword, salt, KeyDerivationPrf.HMACSHA256, iterationCount: 100_000, numBytesRequested: 32);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
              
    }
}
