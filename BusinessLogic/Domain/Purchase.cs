using System.Text.RegularExpressions;

namespace BusinessLogic.Dominio
{
    public class Purchase:IEntity<Purchase.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public Supplier Supplier { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public Purchase(int id,DateTime date, string status, Supplier supplier)
        {
            Id = id;
            Date = date;
            Status = status;
            Supplier = supplier;
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
