using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;

namespace BusinessLogic.Domain
{
    public class Discount : IEntity<Discount.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int ProductQuantity { get; set; }
        public int Percentage { get; set; }
        public Product Product { get; set; }
        public DiscountStatus Status { get; set; } = DiscountStatus.Available;
        public AuditInfo AuditInfo { get ; set ; }

        public Discount(int id, string discription, DateTime expirationDate, int productQuantity, int percentage, Product product, DiscountStatus status)
        {
            Id = id;
            Description = discription;
            ExpirationDate = expirationDate;
            ProductQuantity = productQuantity;
            Percentage = percentage;
            Product = product;
            Status = status;
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
            if (Product == null)
                throw new ArgumentNullException(nameof(Product), "El producto es obligatorio.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");
            Description = data.Description ?? Description;
            ExpirationDate = data.ExpirationDate ?? ExpirationDate;
            ProductQuantity = data.ProductQuantity ?? ProductQuantity;
            Percentage = data.Percentage ?? Percentage;
            Product = data.Product ?? Product;
            Status = data.Status ?? Status;
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }

        public class UpdatableData
        {
            public string? Description { get; set; } = string.Empty;
            public DateTime? ExpirationDate { get; set; }
            public int? ProductQuantity { get; set; }
            public int? Percentage { get; set; }
            public Product? Product { get; set; }
            public DiscountStatus? Status { get; set; }
        }

    }

}
