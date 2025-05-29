namespace BusinessLogic.Dominio
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public Role(int id, string name, List<Permission> permissions)
        {
            Id = id;
            Name = name;
            Permissions = permissions;
        }

        public void Validate()
        {
            if(string.IsNullOrEmpty(Name))
            {
                throw new Exception("El nombre del rol no puede estar vacío");
            }
        }

        public void Update(string roleName)
        {
            Name = roleName;
        }
    }
}
