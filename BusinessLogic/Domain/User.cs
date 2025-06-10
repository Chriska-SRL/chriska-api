using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BusinessLogic.Dominio
{
    public class User:IEntity<User.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean isEnabled { get; set; }
        public Boolean needsPasswordChange { get; set; }
        public Role Role { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();
       
        
        public User(int id, string name, string username, string password, Boolean isEnabled, Boolean needsPasswordChange, Role role,List<Request> requests)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            this.isEnabled = isEnabled;
            this.needsPasswordChange = needsPasswordChange;
            Role = role ?? throw new ArgumentNullException(nameof(role));
            Requests = requests ?? new List<Request>();

            Validate();
        }
        public User(int id) 
        {
            Id = id;
            Name = "Usuario Temporal";
            Username = "usuariotemporal";
            Password = "password.temporal.123";isEnabled = false;
            needsPasswordChange = false;
            Role = new Role(0);
            Requests = new List<Request>();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name), "El nombre es obligatorio.");

            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentNullException(nameof(Username), "El nombre de usuario es obligatorio.");

            if (Username.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Username), "El nombre de usuario no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException(nameof(Password), "La contraseña hash es obligatoria.");

            if (Password.Length > 20 && Password.Length < 8)
                throw new ArgumentOutOfRangeException(nameof(Password), "La contraseña hash no puede superar los 255 caracteres.");

            if (Role == null)
                throw new ArgumentNullException(nameof(Role), "El rol es obligatorio.");
        }
        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(Password), "La contraseña es obligatoria.");

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,30}$");
            if (!regex.IsMatch(password))
                throw new ArgumentException("La contraseña debe tener entre 8 y 30 caracteres, incluyendo al menos una mayúscula, una minúscula, un número y un carácter especial.", nameof(Password));
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            if (data.Role == null)
                throw new ArgumentNullException(nameof(data.Role), "El rol es obligatorio.");

            Name = data.Name ?? Name;
            Username = data.Username ?? Username;
            isEnabled = data.isEnabled;
            Role = data.Role ?? Role;
            needsPasswordChange = data.needsPasswordChange;
            Validate();
        }

        public class UpdatableData
        {
            public string Name { get; set; }
            public string Username { get; set; }
            public Boolean isEnabled { get; set; }
            public Boolean needsPasswordChange { get; set; }
            public Role Role { get; set; }
        }

        public override string ToString() { 
            return $"User(Id: {Id}, Name: {Name}, Username: {Username}, isEnabled: {isEnabled}, needsPasswordChange: {needsPasswordChange},  Role: {Role.ToString()})";
        }
    }
}
