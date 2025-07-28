using BusinessLogic.Common;
using BusinessLogic.Común;
using System.Xml.Linq;

namespace BusinessLogic.Domain
{
    public class Role : IEntity<Role.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        public List<User> Users { get; set; } = new List<User>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Role(string name, string description, List<Permission> permissions)
        {
            Name = name;
            Description = description;
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));

            Validate();
        }
        public Role(int id, string name, string description, List<Permission> permissions, AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            Description = description;
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            AuditInfo = auditInfo ?? throw new ArgumentNullException(nameof(auditInfo));

            Validate();
        }
       
        public Role(int id)
        {
            Id = id;
            Name = "Temporal";
            Description = "_";
            Permissions = new List<Permission>();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre del rol no puede estar vacío.");

            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre del rol no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentNullException(nameof(Description), "La descripción del rol no puede estar vacía.");

            if (Description.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Description), "La descripción del rol no puede superar los 255 caracteres.");
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
            Validate();
        }


        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            if (data.Permissions == null)
                throw new ArgumentNullException(nameof(data.Permissions), "La lista de permisos no puede ser nula.");

            Name = data.Name ?? Name;
            Description = data.Description ?? Description;
            Permissions = data.Permissions ?? Permissions;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
            public List<Permission>? Permissions { get; set; } = new List<Permission>();
        }

        public override string ToString()
        {
            return $"Role(Id: {Id}, Name: {Name}, Description: {Description}, Permissions: [{string.Join(", ", Permissions)}])";
        }
    }
}