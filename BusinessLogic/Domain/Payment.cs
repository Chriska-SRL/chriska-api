using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class Payment : IEntity<Payment.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Payment(int id, DateTime date, decimal amount, string paymentMethod, string note, Supplier supplier)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Note = note;
        }

        public void Validate()
        {
            if (Date == null) throw new Exception("La fecha no puede estar vacía");
            if (Amount <= 0) throw new Exception("El monto debe ser mayor a cero");
           
        }

        public void Update(UpdatableData data)
        {
            Date = data.Date;
            Amount = data.Amount;
            Note = data.Note;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string PaymentMethod { get; set; }
            public string Note { get; set; }
            public Supplier Supplier { get; set; }
        }
    }
}
