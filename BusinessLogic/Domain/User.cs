namespace BusinessLogic.Dominio
{
    public class User:IEntity<User.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean isEnabled { get; set; }
        public Role Role { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();

        public User(int id, string name, string username, string password, Boolean isEnabled, Role role,List<Request> requests)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            this.isEnabled = isEnabled;
            Role = role;
            Requests = requests;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(Username)) throw new Exception("El nombre de usuario es obligatorio");
            if (string.IsNullOrEmpty(Password)) throw new Exception("La contraseña es obligatoria");
            if (Role == null) throw new Exception("El rol es obligatorio");
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name;
            Username = data.Username;
            Password = data.Password;
            this.isEnabled = data.isEnabled;
            Role = data.Role;
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public Boolean isEnabled { get; set; }
            public Role Role { get; set; }
        }
    }
}
