using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PolyWars.Server.Utility.Hashing {
    public class PasswordHashing {

        /// <summary>
        /// Generates a ciphertext and salt from inputs
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltLength">the lenght of desired salt in bytes</param>
        /// <param name="hashAlgo">the HashAlgorithm used to generate the ciphertext</param>
        /// <returns>a HashWithSaltResult object</returns>
        public HashWithSaltResult HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo) {
            byte[] saltBytes = GenerateRandomCryptographicBytes(saltLength);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }

        /// <summary>
        /// Generates a ciphertext and salt from inputs
        /// used to verify credentials
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt">the salt from db</param>
        /// <param name="hashAlgo">the HashAlgorithm used to generate the ciphertext</param>
        /// <returns>a HashWithSaltResult object</returns>
        public HashWithSaltResult HashWithSalt(string password, string salt, HashAlgorithm hashAlgo) {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }

        private byte[] GenerateRandomCryptographicBytes(int keyLength) {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }
    }

    /// <summary>
    /// A simple object for storing a cipher and salt
    /// </summary>
    public class HashWithSaltResult {
        public string Salt { get; }
        public string CipherText { get; set; }

        public HashWithSaltResult(string salt, string cipherText) {
            Salt = salt;
            CipherText = cipherText;
        }
    }
}
