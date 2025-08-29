using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Brand : IEntity<Brand.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public AuditInfo? AuditInfo { get; set; }

        public Brand(string name, string description)
        {
            Name = name;
            Description = description;
            AuditInfo = new AuditInfo();
            Validate();
        }
        public Brand(int id, string name, string description, AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            AuditInfo = auditInfo;
        }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre es obligatorio.");
            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción es obligatoria.");
            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción no puede superar los 255 caracteres.");
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
        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }
        public class UpdatableData:AuditData
        {
            public string? Name { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
           
        }

    }
}
