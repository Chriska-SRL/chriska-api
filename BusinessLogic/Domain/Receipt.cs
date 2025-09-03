using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public abstract class Receipt : IEntity<Receipt.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public PaymentMethod PaymentMethod {  get; set; }
        public AuditInfo? AuditInfo { get; set; }


        //Constructor de creacion
        public Receipt(DateTime date, decimal amount, string notes,PaymentMethod paymentMethod)
        {
            Date = date;
            Amount = amount;
            Notes = notes;
            PaymentMethod = paymentMethod;
            AuditInfo =  new AuditInfo();
            Validate();
        }
        //Constructor de lectura
        public Receipt(int id, DateTime date, decimal amount, string notes, PaymentMethod paymentMethod, AuditInfo? auditInfo)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Notes = notes;
            PaymentMethod = paymentMethod;
            AuditInfo = auditInfo;
        }

        public void Validate()
        {
            if (Date == default)
                throw new ArgumentNullException("La fecha es obligatoria.");

            if (Amount <= 0)
                throw new ArgumentOutOfRangeException("El monto debe ser mayor a cero.");

            if (!string.IsNullOrWhiteSpace(Notes) && Notes.Length > 250)
                throw new ArgumentOutOfRangeException("Las notas no pueden superar los 250 caracteres.");

        }
        public void Update(UpdatableData data)
        {
            Notes = data.Notes ?? Notes;
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }

        public class UpdatableData: AuditData
        {
            public string? Notes { get; set; }
        }
    }
}
