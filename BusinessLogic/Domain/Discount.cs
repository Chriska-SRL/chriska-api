using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class Discount : IEntity<Discount.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public int Percentage { get; set; }
        public DiscountProductType discountProductType { get; set; } = DiscountProductType.All;
        public Brand? Brand { get; set; } = null;
        public SubCategory? SubCategory { get; set; } = null;
        public List<Product> Products { get; set; }
        public DiscountClientType discountClientType { get; set; } = DiscountClientType.All;
        public Zone? Zone { get; set; } = null;
        public List<Client> Clients { get; set; }
        public DiscountStatus Status { get; set; } = DiscountStatus.Available;
        public AuditInfo? AuditInfo { get ; set ; }

        //add constructor
        public Discount(string discription, DateTime expirationDate, int productQuantity, int percentage, List<Product> products, List<Client> clients, DiscountStatus status)
        {
            Description = discription;
            ExpirationDate = expirationDate;
            ProductQuantity = productQuantity;
            Percentage = percentage;
            Products = products ?? throw new ArgumentNullException(nameof(products), "La lista de productos no puede ser nula.");
            Clients = clients ?? throw new ArgumentNullException(nameof(clients), "La lista de clientes no puede ser nula.");
            Status = status;
            Validate();
        }
        //get constructor
        public Discount(int id, string discription, DateTime expirationDate, int productQuantity, int percentage, List<Product> products, List<Client> clients, DiscountStatus status, AuditInfo? auditInfo)
        {
            Id = id;
            Description = discription;
            ExpirationDate = expirationDate;
            ProductQuantity = productQuantity;
            Percentage = percentage;
            Products = products ?? throw new ArgumentNullException(nameof(products), "La lista de productos no puede ser nula.");
            Clients = clients ?? throw new ArgumentNullException(nameof(clients), "La lista de clientes no puede ser nula.");
            Status = status;
            AuditInfo = auditInfo ?? null;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción es obligatoria.");
            if (ExpirationDate < DateTime.Now)
                throw new ArgumentOutOfRangeException(nameof(ExpirationDate), "La fecha de expiración no puede ser en el pasado.");
            if (ProductQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(ProductQuantity), "La cantidad de productos debe ser mayor a cero.");
            if (Percentage < 0 || Percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(Percentage), "El porcentaje debe estar entre 0 y 100.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Description = data.Description ?? Description;
            ExpirationDate = data.ExpirationDate ?? ExpirationDate;
            ProductQuantity = data.ProductQuantity ?? ProductQuantity;
            Percentage = data.Percentage ?? Percentage;
            Products = data.Products ?? Products;
            Clients = data.Clients ?? Clients;
            Status = data.Status ?? Status;

            AuditInfo?.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo?.SetDeleted(userId, location);
        }

        public class UpdatableData: AuditData
        {
            public string? Description { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public int? ProductQuantity { get; set; }
            public int? Percentage { get; set; }
            public List<Product>? Products { get; set; }
            public List<Client>? Clients { get; set; } 
            public DiscountStatus? Status { get; set; }
        }

    }

}
