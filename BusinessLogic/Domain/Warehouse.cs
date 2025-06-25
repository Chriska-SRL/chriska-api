namespace BusinessLogic.Dominio
{
    public class Warehouse : IEntity<Warehouse.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Shelve> Shelves { get; set; } = new List<Shelve>();

        public Warehouse(int id, string name, string description, string address, List<Shelve> shelves)
        {
            Id = id;
            Name = name;
            Description = description;
            Address = address;
            Shelves = shelves ?? throw new ArgumentNullException(nameof(shelves));

            Validate();
        }

        public Warehouse(int id)
        {
            Id = id;
            Name = "Temporal";
            Description = "_";
            Address = "_";
            Shelves = new List<Shelve>();
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

            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentNullException(nameof(Address), "La dirección del almacén no puede estar vacía.");

            if (Address.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Address), "La dirección del almacén no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            Address = data.Address ?? Address;

            Validate();
        }

        public class UpdatableData
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Address { get; set; }
        }

        public override string ToString()
        {
            return $"Warehouse(Id: {Id}, Name: {Name}, Description: {Description}, Address: {Address})";
        }
    }
}
