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
        public string Observation { get; set; }
        public string BankAccount { get; set; }
        public int LoanedCrates { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();
        public Zone Zone { get; set; }
        public List<Receipt> Receipts { get; set; } = new List<Receipt>();

        public Client(int id,string name, string rut, string razonSocial, string address, string mapsAddress, string schedule, string phone, string contactName, string email, string observation, string bankAccount, int loanedCrates, Zone zone,List<Request> requests, List<Receipt> receipts)
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
            Observation = observation;
            BankAccount = bankAccount;
            LoanedCrates = loanedCrates;
            Zone = zone;
            Requests = requests;
            Receipts = receipts;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre no puede estar vacío");
            if (string.IsNullOrEmpty(RUT)) throw new Exception("El RUT no puede estar vacío");
            if (string.IsNullOrEmpty(RazonSocial)) throw new Exception("La razon social no puede estar vacía");
            if (string.IsNullOrEmpty(Address)) throw new Exception("La dirección no puede estar vacía");
            if (string.IsNullOrEmpty(MapsAddress)) throw new Exception("La dirección de Maps no puede estar vacía");
            if (string.IsNullOrEmpty(Schedule)) throw new Exception("El horario no puede estar vacío");
            if (string.IsNullOrEmpty(Phone)) throw new Exception("El teléfono no puede estar vacío");
            if (string.IsNullOrEmpty(ContactName)) throw new Exception("El nombre de contacto no puede estar vacío");
            if (string.IsNullOrEmpty(Email)) throw new Exception("El email no puede estar vacío");
            if (string.IsNullOrEmpty(Observation)) throw new Exception("La observación no puede estar vacía");
            if (string.IsNullOrEmpty(BankAccount)) throw new Exception("La cuenta bancaria no puede estar vacía");
            if (LoanedCrates < 0) throw new Exception("Las cajas prestadas no pueden ser negativas");
        }

        public void Update(UpdatableData updatableData)
        {
            Name = updatableData.Name;
            RUT = updatableData.RUT;
            RazonSocial = updatableData.RazonSocial;
            Address = updatableData.Address;
            MapsAddress = updatableData.MapsAddress;
            Schedule = updatableData.Schedule;
            Phone = updatableData.Phone;
            ContactName = updatableData.ContactName;
            Email = updatableData.Email;
            Observation = updatableData.Observation;
            BankAccount = updatableData.BankAccount;
            LoanedCrates = updatableData.LoanedCrates;
            Zone = updatableData.Zone;
            Validate();
        }
        public class UpdatableData
        {

            public string Name;
            public string RUT;
            public string RazonSocial;
            public string Address;
            public string MapsAddress;
            public string Schedule;
            public string Phone;
            public string ContactName;
            public string Email;
            public string Observation;
            public string BankAccount;
            public int LoanedCrates;
            public Zone Zone;
        }

    }
}
