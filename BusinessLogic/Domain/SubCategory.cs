using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class SubCategory : IEntity<SubCategory.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }
        public AuditInfo? AuditInfo { get; set; }

        public SubCategory(int id, string name, string description, Category? category, AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            AuditInfo = auditInfo;
            Validate();
        }
        public SubCategory(string name, string description, Category category)
        {
            Name = name;
            Description = description;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            AuditInfo = new AuditInfo();

            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException("El nombre no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException("El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException("La descripción no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException("La descripción no puede superar los 255 caracteres.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException("Los datos de actualización no pueden ser nulos.");

            Name = data.Name;
            Description = data.Description;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public override string ToString()
        {
            return $"SubCategory(Id: {Id}, Name: {Name}, Description: {Description}, Category: {Category})";
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
            Validate();
        }
    }
}
