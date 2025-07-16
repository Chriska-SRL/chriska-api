using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class Category : IEntity<Category.UpdatableData>, IAuditable
    {
        private int v1;
        private string v2;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;

            Validate();
        }
        public Category(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            Description = "Descripcion Temporal";
        }

        public Category(int v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Name = data.Name;
            Description = data.Description;

            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public override string ToString()
        {
            return $"Category(Id: {Id}, Name: {Name}, Description: {Description}, SubCategories: {SubCategories.Count})";
        }
    }
}
