using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Boolean isEnabled { get; set; }
        public Role Role { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();

        public User(string name, string username, string password, Boolean isEnabled, Role role)
        {
            Name = name;
            Username = username;
            Password = password;
            this.isEnabled = isEnabled;
            Role = role;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(Username)) throw new Exception("El nombre de usuario es obligatorio");
            if (string.IsNullOrEmpty(Password)) throw new Exception("La contraseña es obligatoria");
            if (Role == null) throw new Exception("El rol es obligatorio");
        }
        public void Update(string name, string username, string password, Boolean isEnabled, Role role)
        {
            Name = name;
            Username = username;
            Password = password;
            this.isEnabled = isEnabled;
            Role = role;
        }
    }
}
