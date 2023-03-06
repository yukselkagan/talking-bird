﻿using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Services
{
    public class PasswordService
    {
        const int keySize = 64;
        const int iterations = 350000;
        private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public static string CreatePasswordHash(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }

    }
}
