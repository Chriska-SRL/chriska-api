using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Category : IEntity<Category.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } 
        public string Description { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
        public AuditInfo? AuditInfo { get; set; } 
        
        
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
            AuditInfo = new AuditInfo();
            Validate();
        }
        public Category(int id, string name, string description,List<SubCategory>? subCategories ,AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            SubCategories = subCategories;
            AuditInfo = auditInfo;

            Validate();
        }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentException("El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentException("La descripción no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentException("Los datos de actualización no pueden ser nulos.");

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
