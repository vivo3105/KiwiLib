using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KiwiLib.Services.Common
{
    internal static class Validation
    {
        /// <summary>
        /// Validates whether the given value is a valid 13-digit ISBN
        /// </summary>
        public static bool IsValidISBN(string value, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                error = "ISBN cannot be empty.";
                return false;
            }
            value = value.ToLower().Replace("-", "");
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{13}$");
            if (!regex.IsMatch(value))
            {
                error = "ISBN must be exactly 13 numeric digits.";
                return false;
            }

            // Validate checksum
            int sum = 0;
            for (int i = 0; i < 13; i++)
            {
                int digit = value[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            if (sum % 10 != 0)
            {
                error = "Invalid ISBN checksum.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generates a random valid ISBN-13 (as a string) for demo purpose
        /// </summary>
        public static string GenerateRandomIsbn()
        {
            Random random = new();
            string prefix = "978"; 
            var digits = prefix + string.Concat(Enumerable.Range(0, 9).Select(_ => random.Next(10)));

            // Calculate checksum (13th digit)
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = digits[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;

            return digits + checkDigit;
        }
    }
}
