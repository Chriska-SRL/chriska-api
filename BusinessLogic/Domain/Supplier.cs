using BusinessLogic.Common;
using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
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
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Observations { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<BankAccount> BankAccounts { get; set; }
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Supplier(string name, string rut, string razonSocial, string address, string mapsAddress, string phone, string contactName, string email,string observations, List<BankAccount> bankAccounts)
        {
            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            MapsAddress = mapsAddress;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observations = observations;
            BankAccounts = bankAccounts ?? new List<BankAccount>();
        }
        public Supplier(int id, string name, string rut, string razonSocial, string address, string mapsAddress, string phone, string contactName, string email, string observations, List<BankAccount> bankAccounts, AuditInfo auditInfo)
        {
            Id = id;
            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            MapsAddress = mapsAddress;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observations = observations;
            BankAccounts = bankAccounts ?? new List<BankAccount>();
            AuditInfo = auditInfo ?? throw new ArgumentNullException(nameof(auditInfo));
        }
        public Supplier(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            RUT = "000000000000";
            RazonSocial = "Razón Social Temporal";
            Address = "Dirección Temporal";
            MapsAddress = "Maps Temporal";
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
                throw new ArgumentNullException(nameof(Name), "El nombre es obligatorio.");
            if (Name.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Name), "El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(RUT))
                throw new ArgumentNullException(nameof(RUT), "El RUT es obligatorio.");
            if (RUT.Length != 12 || !RUT.All(char.IsDigit))
                throw new ArgumentException("El RUT debe tener exactamente 12 dígitos numéricos.", nameof(RUT));

            if (string.IsNullOrWhiteSpace(RazonSocial))
                throw new ArgumentNullException(nameof(RazonSocial), "La razón social es obligatorio"); ;
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(RazonSocial), "El nombre no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentNullException(nameof(Address), "La dirección es obligatoria"); ;
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Address), "La dirección no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(MapsAddress))
                throw new ArgumentNullException(nameof(MapsAddress), "La dirección del mapa es obligatoria"); ;
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(MapsAddress), "La dirección del mapa no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Phone))
                throw new ArgumentNullException(nameof(Phone), "El teléfono es obligatorio.");
            if (!Phone.All(char.IsDigit))
                throw new ArgumentException("El teléfono debe contener solo números.", nameof(Phone));
            if (Phone.Length < 8 || Phone.Length > 9)
                throw new ArgumentOutOfRangeException(nameof(Phone), "El teléfono debe tener entre 8 y 9 dígitos.");

            if (string.IsNullOrWhiteSpace(ContactName))
                throw new ArgumentNullException(nameof(ContactName), "El nombre de contacto es obligatorio.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentNullException(nameof(Email), "El email es obligatorio.");
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailRegex))
                throw new ArgumentException("El email tiene un formato inválido.", nameof(Email));

            if (Observations.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(Observations), "Las observaciones no pueden superar los 255 caracteres.");
        }
        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            RUT = data.RUT ?? RUT;
            RazonSocial = data.RazonSocial ?? RazonSocial;
            Address = data.Address ?? Address;
            MapsAddress = data.MapsAddress ?? MapsAddress;
            Phone = data.Phone ?? Phone;
            ContactName = data.ContactName ?? ContactName;
            Email = data.Email ?? Email;
            Observations = data.Observations ?? Observations;
            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();
        }

        public class UpdatableData:AuditData
        {
            public string? Name { get; set; }
            public string? RUT { get; set; }
            public string? RazonSocial { get; set; }
            public string? Address { get; set; }
            public string? MapsAddress { get; set; }
            public string? Phone { get; set; }
            public string? ContactName { get; set; }
            public string? Email { get; set; }
            public string? Observations { get; set; }
        }
    }
}
