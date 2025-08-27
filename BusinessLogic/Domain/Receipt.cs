using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Receipt : IEntity<Receipt.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public Delivery Delivery { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Receipt(int id,DateTime date, decimal amount, string notes, Delivery delivery, AuditInfo auditInfo)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Notes = notes;
            Delivery = delivery;
            AuditInfo = auditInfo ?? new AuditInfo();
            Validate();
        }

        public void Validate()
        {
            if (Date == default)
                throw new ArgumentNullException(nameof(Date), "La fecha es obligatoria.");

            if (Amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(Amount), "El monto debe ser mayor a cero.");

            if (!string.IsNullOrWhiteSpace(Notes) && Notes.Length > 250)
                throw new ArgumentOutOfRangeException(nameof(Notes), "Las notas no pueden superar los 250 caracteres.");

        }
        public void Update(UpdatableData updatableData)
        {
            Date = updatableData.Date;
            Amount = updatableData.Amount;
            Notes = updatableData.Notes;
            Delivery = updatableData.Delivery;
            AuditInfo = updatableData.AuditInfo ?? new AuditInfo();
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
            public string Notes { get; set; }
            public Delivery Delivery { get; set; }
            public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        }
    }
}
