using BusinessLogic.Común;
using BusinessLogic.Domain;
using System.Text.RegularExpressions;

namespace BusinessLogic.Dominio
{
    public class Client : IEntity<Client.UpdatableData>, IAuditable, IBankUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUT { get; set; }
        public string RazonSocial { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Schedule { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Observations { get; set; }
        public int LoanedCrates { get; set; }
        public string Qualification { get; set; }
        public Zone Zone { get; set; }
        public List<BankAccount> BankAccounts { get; set; } = new();
        public List<Request> Requests { get; set; } = new();
        public List<Receipt> Receipts { get; set; } = new();
        public List<ClientDocument> Documents { get; set; } = new();
        public AuditInfo AuditInfo { get; set; } = new();

        public Client(int id, string name, string rut, string razonSocial, string address, string mapsAddress,
                      string schedule, string phone, string contactName, string email, string observations,
                      List<BankAccount> bankAccounts, int loanedCrates, string qualification, Zone zone)
        {
            Id = id;
            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            MapsAddress = mapsAddress;
            Schedule = schedule;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observations = observations;
            BankAccounts = bankAccounts;
            LoanedCrates = loanedCrates;
            Qualification = qualification;
            Zone = zone;
            Validate();
        }

        public Client(int id)
        {
            Id = id;
            Name = "Nombre Temporal";
            RUT = "000000000000";
            RazonSocial = "Razón Social Temporal";
            Address = "Dirección Temporal";
            MapsAddress = "Maps Temporal";
            Schedule = "Horario Temporal";
            Phone = "099000000";
            ContactName = "Contacto Temporal";
            Email = "email@temporal.com";
            Observations = "Sin observaciones";
            Qualification = "3/5";
            BankAccounts = new List<BankAccount>();
            LoanedCrates = 0;
            Zone = new Zone(99999);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 50)
                throw new ArgumentException("El nombre es obligatorio y no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(RUT) || RUT.Length != 12 || !RUT.All(char.IsDigit))
                throw new ArgumentException("El RUT debe tener exactamente 12 dígitos numéricos.");

            if (string.IsNullOrWhiteSpace(RazonSocial) || RazonSocial.Length > 50)
                throw new ArgumentException("La razón social es obligatoria y no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Address) || Address.Length > 50)
                throw new ArgumentException("La dirección es obligatoria y no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(MapsAddress))
                throw new ArgumentException("La dirección de Maps es obligatoria.");

            if (string.IsNullOrWhiteSpace(Schedule))
                throw new ArgumentException("El horario es obligatorio.");

            if (string.IsNullOrWhiteSpace(Phone) || !Phone.All(char.IsDigit) || Phone.Length < 8 || Phone.Length > 9)
                throw new ArgumentException("El teléfono debe contener entre 8 y 9 dígitos numéricos.");

            if (Phone.Length == 9 && !Phone.StartsWith("09"))
                throw new ArgumentException("El teléfono celular debe comenzar con 09.");

            if (string.IsNullOrWhiteSpace(ContactName))
                throw new ArgumentException("El nombre del contacto es obligatorio.");

            if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El email es obligatorio y debe tener un formato válido.");

            if (string.IsNullOrWhiteSpace(Qualification))
                throw new ArgumentException("La calificación es obligatoria.");

            if (!Regex.IsMatch(Qualification, @"^[1-5]/5$"))
                throw new ArgumentException("La calificación debe tener el formato n/5 (por ejemplo: 3/5).");

            if (LoanedCrates < 0)
                throw new ArgumentOutOfRangeException(nameof(LoanedCrates), "La cantidad de cajones prestados no puede ser negativa.");

     
        }

        public void Update(UpdatableData data)
        {
            Name = data.Name ?? Name;
            RUT = data.RUT ?? RUT;
            RazonSocial = data.RazonSocial ?? RazonSocial;
            Address = data.Address ?? Address;
            MapsAddress = data.MapsAddress ?? MapsAddress;
            Schedule = data.Schedule ?? Schedule;
            Phone = data.Phone ?? Phone;
            ContactName = data.ContactName ?? ContactName;
            Email = data.Email ?? Email;
            Observations = data.Observations ?? Observations;
            LoanedCrates = data.LoanedCrates ?? LoanedCrates;
            Qualification = data.Qualification ?? Qualification;
            Zone = data.Zone ?? Zone;
            BankAccounts = data.BankAccounts ?? BankAccounts;
            Validate();
        }

        public class UpdatableData
        {
            public string? Name;
            public string? RUT;
            public string? RazonSocial;
            public string? Address;
            public string? MapsAddress;
            public string? Schedule;
            public string? Phone;
            public string? ContactName;
            public string? Email;
            public string? Observations;
            public int? LoanedCrates;
            public string? Qualification;
            public Zone? Zone;
            public List<BankAccount>? BankAccounts;
        }
    }
}
