using BusinessLogic.Common;
using BusinessLogic.Domain;
using System.Text.RegularExpressions;

namespace BusinessLogic.Domain
{
    public class Purchase:IEntity<Purchase.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public Supplier Supplier { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<ProductDocument> ProductDocument { get; set; } 
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Purchase(int id,DateTime date, string referece, Supplier supplier,List<Payment> payments,AuditInfo auditInfo)
        {
            Id = id;
            Date = date;
            Reference = referece;
            Supplier = supplier;
            Payments = payments ?? new List<Payment>();
            AuditInfo = auditInfo ?? new AuditInfo();
        }

        public void Validate()
        {
            if (Date == default)
                throw new ArgumentNullException(nameof(Date), "La fecha es obligatoria.");
            if (Date > DateTime.Now)
                throw new ArgumentException("La fecha no puede ser en el futuro.", nameof(Date));

            if (Supplier == null)
                throw new ArgumentNullException(nameof(Supplier), "El proveedor es obligatorio."); 
        }


        public void Update(UpdatableData data)
        {
            Date = data.Date ?? Date;
            Supplier = data.Supplier ?? Supplier;
            AuditInfo = data.AuditInfo ?? new AuditInfo();
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }

        public class UpdatableData
        {
            public DateTime? Date { get; set; }
            public string? Status { get; set; }
            public Supplier? Supplier { get; set; }
            public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        }
    }
}
