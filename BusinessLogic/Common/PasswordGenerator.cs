using System.Text;

namespace BusinessLogic.Common
{
    public static class PasswordGenerator
    {
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string Symbols = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        private static readonly Random _random = new();

        public static string Generate()
        {
            var password = new StringBuilder();

            // Asegurar uno de cada tipo
            password.Append(Uppercase[_random.Next(Uppercase.Length)]);
            password.Append(Lowercase[_random.Next(Lowercase.Length)]);
            password.Append(Digits[_random.Next(Digits.Length)]);
            password.Append(Symbols[_random.Next(Symbols.Length)]);

            string allChars = Uppercase + Lowercase + Digits + Symbols;

            // Completar hasta 12 caracteres
            for (int i = 4; i < 12; i++)
            {
                password.Append(allChars[_random.Next(allChars.Length)]);
            }

            // Mezclar los caracteres para no dejar los primeros fijos
            return new string(password.ToString().OrderBy(_ => _random.Next()).ToArray());
        }
    }
}
