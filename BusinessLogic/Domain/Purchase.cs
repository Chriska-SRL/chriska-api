using BusinessLogic.Común;
using BusinessLogic.Domain;
using System.Text.RegularExpressions;

namespace BusinessLogic.Dominio
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

        public Purchase(int id,DateTime date, string referece, Supplier supplier,List<Payment> payments)
        {
            Id = id;
            Date = date;
            Reference = referece;
            Supplier = supplier;
            Payments = payments ?? new List<Payment>();
        }

        public void Validate()
        {
            if (Date == default)
                throw new ArgumentNullException(nameof(Date), "La fecha es obligatoria.");
            if (Date > DateTime.Now)
                throw new ArgumentException("La fecha no puede ser en el futuro.", nameof(Date));

            if (string.IsNullOrWhiteSpace(Status))
                throw new ArgumentNullException(nameof(Status), "El estado es obligatorio.");
            if (Status.Length > 30)
                throw new ArgumentOutOfRangeException(nameof(Status), "El estado no puede superar los 30 caracteres.");
            if (!Regex.IsMatch(Status, @"^[a-zA-Z\s]+$"))
                throw new ArgumentException("El estado solo puede contener letras y espacios.", nameof(Status));

            if (Supplier == null)
                throw new ArgumentNullException(nameof(Supplier), "El proveedor es obligatorio."); 
        }


        public void Update(UpdatableData data)
        {
            Date = data.Date ?? Date;
            Status = data.Status ?? Status ;
            Supplier = data.Supplier ?? Supplier;
        }
        public class UpdatableData
        {
            public DateTime? Date { get; set; }
            public string? Status { get; set; }
            public Supplier? Supplier { get; set; }
        }
    }
}
