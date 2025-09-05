using BusinessLogic.Common;
using System.Text.RegularExpressions;

namespace BusinessLogic.Domain
{
    public class User : IEntity<User.UpdatableData>, IAuditable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
        public bool NeedsPasswordChange { get; set; }
        public Role Role { get; set; }
        public AuditInfo? AuditInfo { get; set; }

        public User(int id, string name, string username, string password, bool isEnabled, bool needsPasswordChange, Role role, AuditInfo? auditInfo)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            IsEnabled = isEnabled;
            NeedsPasswordChange = needsPasswordChange;
            Role = role;
            AuditInfo = auditInfo;
        }

        public User(string name, string username, string password, bool isEnabled, bool needsPasswordChange, Role role)
        {
            Name = name;
            Username = username;
            Password = password ?? throw new ArgumentException(nameof(password));
            IsEnabled = isEnabled;
            NeedsPasswordChange = needsPasswordChange;
            Role = role ?? throw new ArgumentException(nameof(role));
            AuditInfo = new AuditInfo();
            Validate();
        }

        public User(int id)
        {
            Id = id;
            Name = "Usuario Temporal";
            Username = "usuariotemporal";
            Password = "password.temporal.123";
            IsEnabled = false;
            NeedsPasswordChange = false;
            Role = new Role(0);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("El nombre es obligatorio.");

            if (Name.Length > 50)
                throw new ArgumentException("El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            if (Username.Length > 50)
                throw new ArgumentException("El nombre de usuario no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("La contraseña hash es obligatoria.");

            if (Password.Length < 8 || Password.Length > 255)
                throw new ArgumentException("La contraseña hash debe tener entre 8 y 255 caracteres.");

            if (Role == null)
                throw new ArgumentException("El rol es obligatorio.");
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentException("Los datos de actualización no pueden ser nulos.");

            if (data.Role == null)
                throw new ArgumentException("El rol es obligatorio.");

            Name = data.Name ?? Name;
            Username = data.Username ?? Username;
            IsEnabled = data.IsEnabled;
            Role = data.Role;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña es obligatoria.");

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,30}$");
            if (!regex.IsMatch(password))
                throw new ArgumentException("La contraseña debe tener entre 8 y 30 caracteres, incluyendo al menos una mayúscula, una minúscula, un número y un carácter especial.");
        }

        internal void SetPassword(string password)
        {
            ValidatePassword(password);
            Password = BCrypt.Net.BCrypt.HashPassword(password) ?? throw new ArgumentException("La contraseña no puede ser nula.");
        }

        public class UpdatableData : AuditData
        {
            public string? Name { get; set; }
            public string? Username { get; set; }
            public bool IsEnabled { get; set; }
            public Role Role { get; set; } 
        }

        public override string ToString()
        {
            return $"User(Id: {Id}, Name: {Name}, Username: {Username}, IsEnabled: {IsEnabled}, NeedsPasswordChange: {NeedsPasswordChange}, Role: {Role})";
        }
    }
}
