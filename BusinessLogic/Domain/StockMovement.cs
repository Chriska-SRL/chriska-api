using BusinessLogic.Común.Enums;

namespace BusinessLogic.Dominio
{
    public class StockMovement : IEntity<StockMovement.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public Shelve Shelve { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }

        public StockMovement(int id, DateTime date, int quantity, StockMovementType type, string reason, Shelve shelve, User user, Product product)
        {
            Id = id;
            Date = date;
            Quantity = quantity;
            Type = type;
            Reason = reason;
            Shelve = shelve ?? throw new ArgumentNullException(nameof(shelve));
            User = user ?? throw new ArgumentNullException(nameof(user));
            Product = product ?? throw new ArgumentNullException(nameof(product));

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

            if (Shelve == null)
                throw new ArgumentNullException(nameof(Shelve), "El estante no puede estar vacío.");

            if (User == null)
                throw new ArgumentNullException(nameof(User), "El usuario no puede estar vacío.");

            if (Product == null)
                throw new ArgumentNullException(nameof(Product), "El producto no puede estar vacío.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Date = data.Date;
            Quantity = data.Quantity ?? Quantity;
            Type = data.Type ?? Type;
            Reason = data.Reason ?? Reason;
            Shelve = data.Shelve ?? Shelve;
            User = data.User ?? User;
            Product = data.Product ?? Product;

            Validate();
        }

        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public int? Quantity { get; set; }
            public StockMovementType? Type { get; set; }
            public string? Reason { get; set; }
            public Shelve? Shelve { get; set; }
            public User? User { get; set; }
            public Product? Product { get; set; }
        }

        public override string ToString()
        {
            return $"StockMovement(Id: {Id}, Date: {Date}, Quantity: {Quantity}, Type: {Type}, Reason: {Reason})";
        }
    }
}
