using BusinessLogic.Común;
using BusinessLogic.Común.Enums;

namespace BusinessLogic.Dominio
{
    public class BankAccount : IEntity<BankAccount.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public Bank Bank { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public BankAccount(int id, string accountName, string accountNumber, Bank bank)
        {
            Id = id;
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

        public void Update(UpdatableData data)
        {
            AccountName = data.AccountName ?? AccountName;
            AccountNumber = data.AccountNumber ?? AccountNumber;
            Bank = data.Bank ?? Bank;
        }

        public class UpdatableData
        {
            public string? AccountName { get; set; }
            public string? AccountNumber { get; set; }
            public Bank? Bank { get; set; }
        }
    }
}
