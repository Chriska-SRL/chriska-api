using BusinessLogic.Common;
using BusinessLogic.Común;

namespace BusinessLogic.Domain
{
    public class Shelve : IEntity<Shelve.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Warehouse Warehouse { get; set; }
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public List<Product> Products { get; set; } = new List<Product>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Shelve(string name, string description, Warehouse warehouse, List<StockMovement> stockMovements)
        {
            Name = name;
            Description = description;
            Warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
            StockMovements = stockMovements ?? throw new ArgumentNullException(nameof(stockMovements));

            Validate();
        }
        public Shelve(int id,string name, string description, Warehouse warehouse, List<StockMovement> stockMovements,AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
            StockMovements = stockMovements ?? throw new ArgumentNullException(nameof(stockMovements));
            AuditInfo = auditInfo ?? throw new ArgumentNullException(nameof(auditInfo));

            Validate();
        }

        public Shelve(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El ID del estante debe ser mayor a cero.");

            Id = id;
            Name = "Nombre Temporal";
            Description = "Descripcion Temporal";
            Warehouse = new Warehouse(9999);
            StockMovements = new List<StockMovement>();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre  del estante no puede estar vacía.");

            if (Description.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre del estante no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción del estante no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción del estante no puede superar los 255 caracteres.");

            if (Warehouse == null)
                throw new ArgumentNullException(nameof(Warehouse), "El almacén es obligatorio.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

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
            throw new NotImplementedException();
        }
    }
}
