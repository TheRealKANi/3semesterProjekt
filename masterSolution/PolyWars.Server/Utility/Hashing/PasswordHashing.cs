using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PolyWars.Server.Utility.Hashing {
    public class PasswordHashing {
        public HashWithSaltResult HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo) {
            byte[] saltBytes = GenerateRandomCryptographicBytes(saltLength);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }

        public HashWithSaltResult HashWithSalt(string password, string salt, HashAlgorithm hashAlgo) {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }

        private string GenerateRandomCryptographicKey(int keyLength) {
            return Convert.ToBase64String(GenerateRandomCryptographicBytes(keyLength));
        }

        private byte[] GenerateRandomCryptographicBytes(int keyLength) {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }
    }


    public class HashWithSaltResult {
        public string Salt { get; }
        public string CipherText { get; set; }

        public HashWithSaltResult(string salt, string cipherText) {
            Salt = salt;
            CipherText = cipherText;
        }
    }
}
