using BusinessLogic.Común.Enums;

namespace BusinessLogic.Domain
{
    public class BankAccount 
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public Bank Bank { get; set; }

        public BankAccount(string accountName, string accountNumber, Bank bank)
        {
            AccountName = accountName;
            AccountNumber = accountNumber;
            Bank = bank;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(AccountName))
                throw new ArgumentNullException(nameof(AccountName), "El nombre de la cuenta es obligatorio.");

            if (string.IsNullOrWhiteSpace(AccountNumber))
                throw new ArgumentNullException(nameof(AccountNumber), "El número de cuenta es obligatorio.");

            if (!AccountNumber.All(char.IsDigit))
                throw new ArgumentException("El número de cuenta debe contener solo dígitos.");

            if (AccountNumber.Length < 10 || AccountNumber.Length > 14)
                throw new ArgumentOutOfRangeException(nameof(AccountNumber), "El número de cuenta debe tener entre 10 y 14 dígitos.");
        }

    }
}
