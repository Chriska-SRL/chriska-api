using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class SubCategory : IEntity<SubCategory.UpdatableData>, IAuditable
    {
        private int v1;
        private string v2;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public SubCategory(int id, string name, string description, Category category, AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            AuditInfo = auditInfo ?? throw new ArgumentNullException(nameof(auditInfo));

            Validate();
        }
        public SubCategory(int id, string name, string description, Category category)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category ?? throw new ArgumentNullException(nameof(category));

            Validate();
        }

        public SubCategory(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            Description = "Descripción Temporal";
            Category = new Category(9999);
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

            if (Category == null)
                throw new ArgumentNullException(nameof(Category), "La categoría es obligatoria.");
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
            return $"SubCategory(Id: {Id}, Name: {Name}, Description: {Description}, Category: {Category})";
        }
    }
}
