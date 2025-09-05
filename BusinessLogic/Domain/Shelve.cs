using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Shelve : IEntity<Shelve.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Warehouse? Warehouse { get; set; }
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public List<Product> Products { get; set; } = new List<Product>();
        public AuditInfo? AuditInfo { get; set; } = new AuditInfo();

        public Shelve(string name, string description, Warehouse warehouse)
        {
            Name = name;
            Description = description;
            Warehouse = warehouse ?? throw new ArgumentException(nameof(warehouse));
            AuditInfo = new AuditInfo();
            Validate();
        }
        public Shelve(int id,string name, string description, Warehouse? warehouse,AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Warehouse = warehouse;
            AuditInfo = auditInfo;

            Validate();
        }

        public Shelve(int id, string name, string description, Warehouse warehouse, List<StockMovement> stockMovements, AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Warehouse = warehouse ?? throw new ArgumentException(nameof(warehouse));
            StockMovements = stockMovements ?? throw new ArgumentException(nameof(stockMovements));
            AuditInfo = auditInfo ?? throw new ArgumentException(nameof(auditInfo));

            Validate();
        }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("El nombre del estante no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentException("El nombre del estante no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción del estante no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentException("La descripción del estante no puede superar los 255 caracteres.");

        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentException("Los datos de actualización no pueden ser nulos.");

            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            Warehouse = data.Warehouse ?? Warehouse;
            AuditInfo.SetUpdated(data.UserId, data.Location);

            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public Warehouse? Warehouse { get; set; }
        }

        public override string ToString()
        {
            return $"Shelve(Id: {Id}, Name: {Name}, Description: {Description}, WarehouseId: {Warehouse?.Id})";
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }
    }
}
