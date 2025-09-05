using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class StockMovement : IEntity<StockMovement.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public RasonType RasonType { get; set; }
        public string Reason { get; set; } = string.Empty;
        public User User { get; set; }
        public Product Product { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public StockMovement(DateTime date, decimal quantity, StockMovementType type, RasonType rasonType, string reason, User user, Product product)
        {
            Date = date;
            Quantity = quantity;
            Type = type;
            RasonType = rasonType;
            Reason = reason;
            User = user ?? throw new ArgumentException(nameof(user));
            Product = product ?? throw new ArgumentException(nameof(product));
            AuditInfo = new AuditInfo();
            Validate();
        }
        public StockMovement(int id, DateTime date, decimal quantity, StockMovementType type, RasonType rasonType, string reason, User user, Product product,AuditInfo auditInfo)
        {
            Id = id;
            Date = date;
            Quantity = quantity;
            Type = type;
            RasonType = rasonType;
            Reason = reason;
            User = user;
            Product = product;
            AuditInfo = auditInfo;

        }

        public void Validate()
        {
            if (Quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");

            if (!Enum.IsDefined(typeof(StockMovementType), Type))
                throw new ArgumentException("Tipo de unidad inválido.");

            if (Quantity % 0.5m != 0)
                throw new ArgumentException("La cantidad debe ser múltiplo de 0.5 (ej: 0.5, 1, 1.5, 2...).");

            if (string.IsNullOrWhiteSpace(Reason))
                throw new ArgumentException("El motivo del movimiento no puede estar vacío.");

            if (Reason.Length > 255)
                throw new ArgumentException("El motivo del movimiento no puede superar los 255 caracteres.");

            if (User == null)
                throw new ArgumentException("El usuario no puede estar vacío.");

            if (Product == null)
                throw new ArgumentException("El producto no puede estar vacío.");
        }

        public void Update(UpdatableData data)
        {
        }

        public class UpdatableData
        {
        }

        public override string ToString()
        {
            return $"StockMovement(Id: {Id}, Date: {Date}, Quantity: {Quantity}, Type: {Type}, Reason: {Reason})";
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }
    }
}
