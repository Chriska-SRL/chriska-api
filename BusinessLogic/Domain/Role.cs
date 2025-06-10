namespace BusinessLogic.Dominio
{
    public class Role:IEntity<Role.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public Role(int id, string name, string description, List<Permission> permissions)
        {
            Id = id;
            Name = name;
            Description = description;
            Permissions = permissions;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre del rol no puede estar vacío");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
            Description = data.Description;
            Permissions = data.Permissions ?? new List<Permission>();
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; }
            public string Description { get; set; } = string.Empty;
            public List<Permission> Permissions { get; set; } = new List<Permission>();
        }

       public override string ToString()
        {
            return $"Role(Id: {Id}, Name: {Name}, Description: {Description}, Permissions: [{string.Join(", ", Permissions)}])";
        }
    }
}
