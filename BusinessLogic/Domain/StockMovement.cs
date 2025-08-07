using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class StockMovement : IEntity<StockMovement.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public User User { get; set; }
        public Product Product { get; set; }
        public StockMovementType StockMovementType { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public StockMovement(DateTime date, int quantity, StockMovementType type, string reason, User user, Product product)
        {
            Date = date;
            Quantity = quantity;
            Type = type;
            Reason = reason;
            User = user ?? throw new ArgumentNullException(nameof(user));
            Product = product ?? throw new ArgumentNullException(nameof(product));
            AuditInfo = new AuditInfo();
            Validate();
        }
        public StockMovement(int id, DateTime date, int quantity, StockMovementType type, string reason, User user, Product product,AuditInfo auditInfo)
        {
            Id = id;
            Date = date;
            Quantity = quantity;
            Type = type;
            Reason = reason;
            User = user;
            Product = product;
            AuditInfo = auditInfo;

            Validate();
        }

        public void Validate()
        {
            if (Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(Quantity), "La cantidad debe ser mayor a cero.");

            if (!Enum.IsDefined(typeof(StockMovementType), Type))
                throw new ArgumentOutOfRangeException(nameof(Type), "Tipo de unidad inválido.");

            if (string.IsNullOrWhiteSpace(Reason))
                throw new ArgumentNullException(nameof(Reason), "El motivo del movimiento no puede estar vacío.");

            if (Reason.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Reason), "El motivo del movimiento no puede superar los 255 caracteres.");

            if (User == null)
                throw new ArgumentNullException(nameof(User), "El usuario no puede estar vacío.");

            if (Product == null)
                throw new ArgumentNullException(nameof(Product), "El producto no puede estar vacío.");
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
