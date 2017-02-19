using System;
using System.Globalization;
using NodaTime;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Utilities;

namespace NanoFabric.IdentityServer.Services
{
    public class PasswordService : IPasswordService
    {
        private const int StartYear = 2000;
        private const int StartCount = 1000;
        public const char PasswordHashingIterationCountSeparator = '.';
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 256;
        private const int IterationCount = 10000;


        public string HashPassword(string password)
        {
            var count = IterationCount;
            if (count <= 0)
            {
                count = GetIterationsFromYear(GetCurrentYear());
            }
            var result = CryptographyUtility.HashPassword(password, count);
            return EncodeIterations(count) + PasswordHashingIterationCountSeparator + result;
        }

        public bool ValidatePassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var meetsLengthRequirements = password.Length >= MinPasswordLength &&
                                          password.Length <= MaxPasswordLength;
            var hasLetter = false;
            var hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (var c in password)
                {
                    if (char.IsLetter(c)) hasLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }

            var isValid = meetsLengthRequirements
                          && hasLetter
                          && hasDecimalDigit
                ;
            return isValid;
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword.Contains(PasswordHashingIterationCountSeparator.ToString()))
            {
                var parts = hashedPassword.Split(PasswordHashingIterationCountSeparator);
                if (parts.Length != 2) return false;

                var count = DecodeIterations(parts[0]);
                if (count <= 0) return false;

                hashedPassword = parts[1];

                return CryptographyUtility.VerifyHashedPassword(hashedPassword, password, count);
            }
            return CryptographyUtility.VerifyHashedPassword(hashedPassword, password);
        }

        // from OWASP : https://www.owasp.org/index.php/Password_Storage_Cheat_Sheet
        public int GetIterationsFromYear(int year)
        {
            if (year > StartYear)
            {
                var diff = (year - StartYear) / 2;
                var mul = (int)Math.Pow(2, diff);
                var count = StartCount * mul;
                // if we go negative, then we wrapped (expected in year ~2044). 
                // Int32.Max is best we can do at this point
                if (count < 0) count = int.MaxValue;
                return count;
            }
            return StartCount;
        }

        private static int GetCurrentYear()
        {
            return SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc().Year;
        }

        private string EncodeIterations(int count)
        {
            return count.ToString("X");
        }

        private int DecodeIterations(string prefix)
        {
            int val;
            if (int.TryParse(prefix, NumberStyles.HexNumber, null, out val))
            {
                return val;
            }
            return -1;
        }
    }
}
