using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shared
{
    public static class ValidationHelper
    {
        public static bool IsCnpj(string cnpj)
        {
            if (String.IsNullOrWhiteSpace(cnpj)) return false;
            if (cnpj.Any(c => (c < '0') || (c > '9'))) return false;
            cnpj = cnpj.PadLeft(14, '0');
            int[] multiplicador1 = new int[12] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            int[] multiplicador2 = new int[13] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsValidGuid(string guid)
        {
            if (!Guid.TryParse(guid, out Guid guidParsed) || IsEmptyGuid(guidParsed))
                return false;

            return true;
        }

        private static bool IsEmptyGuid(Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool IsValidGuid(Guid guid)
        {
            return !IsEmptyGuid(guid);
        }

        public static bool IsGtin(string gtin)
        {
            if (String.IsNullOrWhiteSpace(gtin)) return false;

            if (gtin.Count() > 14) return false;

            if (Regex.Matches(gtin, @"[a-zA-Z]").Count > 0) return false;

            gtin = new String(gtin.Reverse().ToArray());
            var digit = gtin.First().ToString();
            gtin = gtin.Substring(1);
            int[] mult = Enumerable.Range(0, gtin.Length)
                .Select(i => ((int) char.GetNumericValue(gtin[i])) * ((i % 2 == 0) ? 3 : 1))
                .ToArray(); // STEP 1: without check digit, "Multiply value of each position" by 3 or 1
            int sum = mult.Sum(); // STEP 2: "Add results together to create sum"
            return (10 - (sum % 10)) % 10 == int.Parse(digit);
        }

        public static bool NumberHasLetters(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            if (Regex.Matches(value, @"[a-zA-Z]").Count > 0) return true;

            return false;
        }
        
    }
}