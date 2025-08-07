using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Warehouse : IEntity<Warehouse.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Shelve> Shelves { get; set; } = new List<Shelve>();
        public AuditInfo? AuditInfo { get; set; } = new AuditInfo();

        public Warehouse(int id, string name, string description, List<Shelve> shelves,AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Shelves = shelves;
            AuditInfo = auditInfo;

            Validate();
        }

        public Warehouse(string name, string description)
        {
            Name = name;
            Description = description;
            AuditInfo = new AuditInfo();
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre del almacén no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre del almacén no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción del almacén no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción del almacén no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            AuditInfo.SetUpdated(data.UserId, data.Location);

            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
        }

        public override string ToString()
        {
            return $"Warehouse(Id: {Id}, Name: {Name}, Description: {Description}";
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
            Validate();
        }
    }
}
