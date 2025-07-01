using BusinessLogic.Común.Enums;
using System.Text.RegularExpressions;

namespace BusinessLogic.Dominio
{
    public class Client:IEntity<Client.UpdatableData>
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
        public Bank Bank { get; set; }
        public string BankAccount { get; set; }
        public int LoanedCrates { get; set; }
        public Zone Zone { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();       
        public List<Receipt> Receipts { get; set; } = new List<Receipt>();

        public Client(int id,string name, string rut, string razonSocial, string address, string mapsAddress, string schedule, string phone, string contactName, string email, string observations,Bank bank ,string bankAccount, int loanedCrates, Zone zone)
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
            Bank = bank;
            BankAccount = bankAccount;
            LoanedCrates = loanedCrates;
            Zone = zone;
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
            Bank = Bank.Andbank;
            BankAccount = "0000000000";
            LoanedCrates = 0;
            Zone = new Zone(0, "Zona Temporal", "Descripción Temporal");
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
                throw new ArgumentNullException(nameof(RazonSocial), "La razón social es obligatoria.");
            if (RazonSocial.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(RazonSocial), "La razón social no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentNullException(nameof(Address), "La dirección es obligatoria.");
            if (Address.Length > 50)
                throw new ArgumentOutOfRangeException(nameof(Address), "La dirección no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(MapsAddress))
                throw new ArgumentNullException(nameof(MapsAddress), "La dirección de Maps es obligatoria.");

            if (string.IsNullOrWhiteSpace(Schedule))
                throw new ArgumentNullException(nameof(Schedule), "El horario es obligatorio.");

            if (string.IsNullOrWhiteSpace(Phone))
                throw new ArgumentNullException(nameof(Phone), "El teléfono es obligatorio.");
            if (!Phone.All(char.IsDigit))
                throw new ArgumentException("El teléfono debe contener solo dígitos.", nameof(Phone));
            if (Phone.Length < 8 || Phone.Length > 9)
                throw new ArgumentOutOfRangeException(nameof(Phone), "El teléfono debe tener entre 8 y 9 dígitos.");
            if (Phone.Length == 9 && !Phone.StartsWith("09"))
                throw new ArgumentException("El teléfono celular debe comenzar con 09.", nameof(Phone));

            if (string.IsNullOrWhiteSpace(ContactName))
                throw new ArgumentNullException(nameof(ContactName), "El nombre del contacto es obligatorio.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentNullException(nameof(Email), "El email es obligatorio.");

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailRegex))
                throw new ArgumentException("El email tiene un formato inválido.", nameof(Email));

            if (string.IsNullOrWhiteSpace(BankAccount))
                throw new ArgumentNullException(nameof(BankAccount), "La cuenta bancaria es obligatoria.");
            if (!BankAccount.All(char.IsDigit))
                throw new ArgumentException("La cuenta bancaria debe contener solo dígitos.", nameof(BankAccount));
            if (BankAccount.Length < 10 || BankAccount.Length > 14)
                throw new ArgumentOutOfRangeException(nameof(BankAccount), "La cuenta bancaria debe tener entre 10 y 14 dígitos.");
            if (LoanedCrates < 0)
                throw new ArgumentOutOfRangeException(nameof(LoanedCrates), "La cantidad de cajones prestados no puede ser negativa.");
        }

        public void Update(UpdatableData updatableData)
        {
            Name = updatableData.Name ?? Name;
            RUT = updatableData.RUT ?? RUT;
            RazonSocial = updatableData.RazonSocial ?? RazonSocial;
            Address = updatableData.Address ?? Address;
            MapsAddress = updatableData.MapsAddress ?? MapsAddress;
            Schedule = updatableData.Schedule ?? Schedule;
            Phone = updatableData.Phone ?? Phone;
            ContactName = updatableData.ContactName ?? ContactName;
            Email = updatableData.Email ?? Email;
            Observations = updatableData.Observations ?? Observations;
            Bank = updatableData.Bank ?? Bank;
            BankAccount = updatableData.BankAccount ?? BankAccount;
            LoanedCrates = updatableData.LoanedCrates ?? LoanedCrates;
            Zone = updatableData.Zone ?? Zone;
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
            public Bank? Bank;
            public string? BankAccount;
            public int? LoanedCrates;
            public Zone? Zone;
        }

    }
}
