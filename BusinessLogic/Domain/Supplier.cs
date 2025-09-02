using BusinessLogic.Common;
using System.Text.RegularExpressions;

namespace BusinessLogic.Domain
{
    public class Supplier:IEntity<Supplier.UpdatableData>, IAuditable,IBankUser
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string RUT { get; set; }
        public string RazonSocial { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Observations { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<BankAccount> BankAccounts { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Supplier(string name, string rut, string razonSocial, string address, Location location, string phone, string contactName, string email,string observations, List<BankAccount> bankAccounts)
        {
            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            Location = location;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observations = observations;
            BankAccounts = bankAccounts ?? new List<BankAccount>();
            AuditInfo = new AuditInfo();
        }
        public Supplier(int id, string name, string rut, string razonSocial, string address, Location location, string phone, string contactName, string email, string observations, List<BankAccount> bankAccounts, AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            Location = location;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observations = observations;
            BankAccounts = bankAccounts ?? new List<BankAccount>();
            AuditInfo = auditInfo;
        }
        public Supplier(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            RUT = "000000000000";
            RazonSocial = "Razón Social Temporal";
            Address = "Dirección Temporal";
            Location = new Location(0,0);
            Phone = "099000000";
            ContactName = "Contacto Temporal";
            Email = "email@temporal.com";
            Observations = "Sin observaciones";
            Purchases = new List<Purchase>();
            Products = new List<Product>();
            BankAccounts = new List<BankAccount>();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException("El nombre es obligatorio.");
            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException("El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(RUT))
                throw new ArgumentNullException("El RUT es obligatorio.");
            if (RUT.Length != 12 || !RUT.All(char.IsDigit))
                throw new ArgumentException("El RUT debe tener exactamente 12 dígitos numéricos.");

            if (string.IsNullOrWhiteSpace(RazonSocial))
                throw new ArgumentNullException("La razón social es obligatorio"); ;
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException("El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentNullException("La dirección es obligatoria"); ;
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException("La dirección no puede superar los 50 caracteres.");
            if (string.IsNullOrWhiteSpace(Phone))
                throw new ArgumentNullException("El teléfono es obligatorio.");
            if (!Phone.All(char.IsDigit))
                throw new ArgumentException("El teléfono debe contener solo números.");
            if (Phone.Length < 8 || Phone.Length > 9)
                throw new ArgumentOutOfRangeException("El teléfono debe tener entre 8 y 9 dígitos.");

            if (string.IsNullOrWhiteSpace(ContactName))
                throw new ArgumentNullException("El nombre de contacto es obligatorio.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentNullException(nameof(Email), "El email es obligatorio.");
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailRegex))
                throw new ArgumentException("El email tiene un formato inválido.");

            if (Observations.Length > 255)
                throw new ArgumentOutOfRangeException("Las observaciones no pueden superar los 255 caracteres.");
        }
        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            RUT = data.RUT ?? RUT;
            RazonSocial = data.RazonSocial ?? RazonSocial;
            Address = data.Address ?? Address;
            Location = data.Location;
            Phone = data.Phone ?? Phone;
            ContactName = data.ContactName ?? ContactName;
            Email = data.Email ?? Email;
            Observations = data.Observations ?? Observations;
            BankAccounts = data.BankAccounts ?? BankAccounts;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; }
            public string? RUT { get; set; }
            public string? RazonSocial { get; set; }
            public string? Address { get; set; }
            public Location? Location { get; set; }
            public string? Phone { get; set; }
            public string? ContactName { get; set; }
            public string? Email { get; set; }
            public string? Observations { get; set; }
            public List<BankAccount>? BankAccounts { get; set; } 
        }
    }
}
