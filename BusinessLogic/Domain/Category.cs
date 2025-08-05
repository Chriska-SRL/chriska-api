using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Category : IEntity<Category.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } 
        public string Description { get; set; }
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        
        
        public Category(string name, string description)
        {
            Name = name;
            Description = description;

            Validate();
        }
        public Category(int id, string name, string description,List<SubCategory> subCategories ,AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            SubCategories = subCategories ?? throw new ArgumentNullException(nameof(subCategories), "La lista de subcategorías no puede ser nula.");
            AuditInfo = auditInfo;

            Validate();
        }
        public Category(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            Description = "Descripcion Temporal";
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
            AuditInfo.SetUpdated(data.UserId, data.Location);

            Validate();
        }

        public class UpdatableData: AuditData
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;

        }

        public override string ToString()
        {
            return $"Category(Id: {Id}, Name: {Name}, Description: {Description})";
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
            Validate();
        }
    }
}
