using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace NanoFabric.IdentityServer.Utilities
{
    public static class CryptographyUtility
    {
        private const int PBKDF2IterationCount = 1000; // default for Rfc2898DeriveBytes
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits
        static readonly char[] Base64Padding = { '=' };

        public static string CreateUniqueId(int length = 16)
        {
            var bytes = new byte[length];
            //new RNGCryptoServiceProvider().GetBytes(bytes);

            return ByteArrayToString(bytes);
        }

        public static string CreateUrlSafeUniqueId(int length = 16)
        {
            if (length < 1)
            {
                throw new ArgumentException($"The value of [{length}] must be positive.");
            }
            var bytes = new byte[length];
            //new RNGCryptoServiceProvider().GetBytes(bytes);
            var returnValue = Convert.ToBase64String(bytes)
                                    .TrimEnd(Base64Padding)
                                    .Replace('+', '-')
                                    .Replace('/', '_');

            return returnValue;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public static string Hash(byte[] input, string algorithm = "sha256")
        {
            if (input == null)
            {
                throw new ArgumentNullException($"The hash input [{nameof(input)}] cannot be null.");
            }
            return string.Empty;
            //using (var alg = HashAlgorithm.Create(algorithm))
            //{
            //    if (alg != null)
            //    {
            //        var hashData = alg.ComputeHash(input);
            //        return BinaryToHex(hashData);
            //    }
            //    throw new InvalidOperationException();
            //}
        }

        public static string Hash(string input, string algorithm = "sha256")
        {
            if (input == null)
            {
                throw new ArgumentNullException($"The hash input [{nameof(input)}] cannot be null.");
            }

            return Hash(Encoding.UTF8.GetBytes(input), algorithm);
        }

        public static string BinaryToHex(byte[] data)
        {
            var hex = new char[data.Length * 2];

            for (var iter = 0; iter < data.Length; iter++)
            {
                var hexChar = ((byte)(data[iter] >> 4));
                hex[iter * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                hexChar = ((byte)(data[iter] & 0xF));
                hex[(iter * 2) + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
            }
            return new string(hex);
        }

        public static byte[] GenerateSaltInternal(int byteLength = SaltSize)
        {
            var buf = new byte[byteLength];
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    rng.GetBytes(buf);
            //}
            return buf;
        }

        public static string GenerateSalt(int byteLength = SaltSize)
        {
            return Convert.ToBase64String(GenerateSaltInternal(byteLength));
        }

        public static string HashPassword(string password, int iterationCount = PBKDF2IterationCount)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, iterationCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password,
            int iterationCount = PBKDF2IterationCount)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterationCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
