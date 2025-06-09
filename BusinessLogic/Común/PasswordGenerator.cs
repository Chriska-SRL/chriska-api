using System.Text;

namespace BusinessLogic.Común
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
            string chars = Uppercase + Lowercase + Digits + Symbols;

            var password = new StringBuilder(12);
            for (int i = 0; i < 12; i++)
            {
                var index = _random.Next(chars.Length);
                password.Append(chars[index]);
            }

            return password.ToString();
        }
    }
}
