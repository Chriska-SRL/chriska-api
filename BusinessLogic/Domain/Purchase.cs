using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Purchase : SupplierDocument
    {
        public string? InvoiceNumber { get; set; }

        public Purchase(string? observation, User user, List<ProductItem>? productItems, Supplier supplier, string? invoiceNumber = null)
            : base(observation, user, productItems, supplier)
        {
            InvoiceNumber = invoiceNumber;
            Validate();
        }

        public Purchase(int id, DateTime date, string observations, User? user, List<ProductItem> productItems, Supplier? supplier, AuditInfo? auditInfo, string? invoiceNumber = null)
            : base(id, date, observations, user, productItems, supplier, auditInfo)
        {
            if (Date == default)
                throw new ArgumentNullException("La fecha es obligatoria.");
            if (Date > DateTime.Now)
                throw new ArgumentException("La fecha no puede ser en el futuro.");

            if (Supplier == null)
                throw new ArgumentNullException("El proveedor es obligatorio."); 
        }

        public override void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Observations) && Observations.Length > 255)
                throw new ArgumentOutOfRangeException("La observación no puede superar los 255 caracteres.");
            if (Supplier == null) throw new ArgumentNullException("El proveedor es obligatorio.");
            if (!string.IsNullOrWhiteSpace(InvoiceNumber) && InvoiceNumber.Length > 30)
                throw new ArgumentOutOfRangeException("El número de factura no puede superar los 30 caracteres.");
        }

        public class UpdatableData : AuditData
        {
            public string? Observations { get; set; }
            public string? InvoiceNumber { get; set; }
            public List<ProductItem>? ProductItems { get; set; }
        }

        public void Update(UpdatableData data)
        {
            Observations = data.Observations ?? Observations;
            InvoiceNumber = data.InvoiceNumber ?? InvoiceNumber;
            if (data.ProductItems != null)
                ProductItems = data.ProductItems;
            AuditInfo?.SetUpdated(data.UserId, data.Location);
            Validate();
        }
    }
}
