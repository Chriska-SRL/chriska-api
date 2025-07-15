using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class BankAccount:IEntity<BankAccount.UpdatableData>,IAuditable
    {
        public int Id { get; set; }
        public string Bank { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public BankAccount(int id, string bank, string number)
        {
            Id = id;
            Bank = bank;
            Number = number;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Bank))
                throw new ArgumentNullException(nameof(Bank), "El banco es obligatorio.");

            if (string.IsNullOrWhiteSpace(Number))
                throw new ArgumentNullException(nameof(Number), "El número de cuenta es obligatorio.");

            if (!Number.All(char.IsDigit))
                throw new ArgumentException("El número de cuenta debe contener solo dígitos.");

            if (Number.Length < 10 || Number.Length > 14)
                throw new ArgumentOutOfRangeException(nameof(Number), "El número de cuenta debe tener entre 10 y 14 dígitos.");
        }
    public void Update(UpdatableData data)
    {
        Bank = data.Bank ?? Bank;
        Number = data.Number ?? Number;
    }
    public class UpdatableData
    {
        public string? Bank { get; set; }
        public string? Number { get; set; }
        }
        
    }
}
