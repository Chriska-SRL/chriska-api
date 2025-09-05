using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using System.Net;

namespace BusinessLogic.Domain
{
    public class Discount : IEntity<Discount.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Percentage { get; set; }
        public Brand? Brand { get; set; }
        public SubCategory? SubCategory { get; set; }
        public List<Product> Products { get; set; } = new();
        public Zone? Zone { get; set; }
        public List<Client> Clients { get; set; } = new();
        public DiscountStatus Status { get; set; } = DiscountStatus.Available;
        public AuditInfo? AuditInfo { get; set; }

        // Constructor para alta
        public Discount(
            string description,
            DateTime expirationDate,
            int productQuantity,
            decimal percentage,
            Brand? brand,
            SubCategory? subCategory,
            Zone? zone,
            List<Product> products,
            List<Client> clients,
            DiscountStatus status)
        {
            Description = description;
            ExpirationDate = expirationDate;
            ProductQuantity = productQuantity;
            Percentage = percentage;
            Brand = brand;
            SubCategory = subCategory;
            Zone = zone;
            Products = products ?? new();
            Clients = clients ?? new();
            Status = status;
            AuditInfo = new AuditInfo();
            Validate();
        }

        // Constructor para lecturas
        public Discount(
            int id,
            string description,
            DateTime expirationDate,
            int productQuantity,
            decimal percentage,
            List<Product> products,
            List<Client> clients,
            DiscountStatus status,
            Brand? brand,
            SubCategory? subCategory,
            Zone? zone,
            AuditInfo? auditInfo)
        {
            Id = id;
            Description = description;
            ExpirationDate = expirationDate;
            ProductQuantity = productQuantity;
            Percentage = percentage;
            Products = products ?? new();
            Clients = clients ?? new();
            Status = status;
            Brand = brand;
            SubCategory = subCategory;
            Zone = zone;
            AuditInfo = auditInfo;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción es obligatoria.");
                throw new ArgumentNullException("La descripción es obligatoria.");
            if (Description.Length > 255)
                throw new ArgumentException("La descripción no puede superar los 255 caracteres.");
            if (ExpirationDate <= DateTime.Now)
                throw new ArgumentException("La fecha de expiración debe ser futura.");
            if (ProductQuantity <= 0)
                throw new ArgumentException("La cantidad de productos debe ser mayor a cero.");
            if (Percentage < 0 || Percentage > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100.");

            // Integridad: cliente/zona
            bool hasClients = Clients.Count > 0;
            bool hasZone = Zone is not null;
            if (!hasClients && !hasZone)
                throw new ArgumentException("Debe asignar al menos un cliente o una zona.");
            if (hasClients && hasZone)
                throw new ArgumentException("No se puede asignar zona y clientes a la vez.");

            // Integridad: productos/marca/subcategoría
            bool hasProducts = Products.Count > 0;
            bool hasBrand = Brand is not null;
            bool hasSubCategory = SubCategory is not null;
            if (!hasProducts && !hasBrand && !hasSubCategory)
                throw new ArgumentException("Debe asignar al menos un producto, una marca o una subcategoría.");
            if (hasProducts && (hasBrand || hasSubCategory))
                throw new ArgumentException("No se puede asignar productos y marca/subcategoría a la vez.");
        }

        public void Update(UpdatableData data)
        {
            if (data is null) throw new ArgumentException(nameof(data));

            Description = data.Description ?? Description;
            ExpirationDate = data.ExpirationDate ?? ExpirationDate;
            ProductQuantity = data.ProductQuantity ?? ProductQuantity;
            Percentage = data.Percentage ?? Percentage;
            Brand = data.Brand ?? Brand;
            SubCategory = data.SubCategory ?? SubCategory;
            Zone = data.Zone ?? Zone;
            Products = data.Products ?? Products;
            Clients = data.Clients ?? Clients;
            Status = data.Status ?? Status;

            AuditInfo?.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location) => AuditInfo?.SetDeleted(userId, location);

        public class UpdatableData : AuditData
        {
            public string? Description { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public int? ProductQuantity { get; set; }
            public decimal? Percentage { get; set; }
            public Brand? Brand { get; set; }
            public SubCategory? SubCategory { get; set; }
            public List<Product>? Products { get; set; }
            public Zone? Zone { get; set; }
            public List<Client>? Clients { get; set; }
            public DiscountStatus? Status { get; set; }
        }
    }
}
