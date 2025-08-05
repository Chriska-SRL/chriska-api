using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Payment : IEntity<Payment.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Payment(int id, DateTime date, decimal amount, string note,AuditInfo auditInfo)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Note = note;
            AuditInfo = auditInfo ?? new AuditInfo();
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
            AuditInfo = data.AuditInfo ?? new AuditInfo();
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }

        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Note { get; set; }
            public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        }
    }
}
