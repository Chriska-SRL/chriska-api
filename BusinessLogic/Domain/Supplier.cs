namespace BusinessLogic.Dominio
{
    public class Supplier:IEntity<Supplier.UpdatableData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUT { get; set; }
        public string RazonSocial { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string BankAccount { get; set; }
        public string Observations { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Day> DaysToDeliver { get; set; } = new List<Day>();

        public Supplier(int id, string name, string rut, string razonSocial, string address, string mapsAddress, string phone, string contactName, string email,string bankAccount, string observations, List<Product> products, List<Payment> payments, List<Purchase> purchases, List<Day> daysToDeliver)
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
            BankAccount = bankAccount;
            Observations = observations;
            Products = products;
            Payments = payments;
            Purchases = purchases;
            DaysToDeliver = daysToDeliver;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(RUT)) throw new Exception("El RUT es obligatorio");
            if (string.IsNullOrEmpty(RazonSocial)) throw new Exception("La razon social es obligatoria");
            if (string.IsNullOrEmpty(Address)) throw new Exception("La direccion es obligatoria");
            if (string.IsNullOrEmpty(Phone)) throw new Exception("El telefono es obligatorio");
            if (string.IsNullOrEmpty(ContactName)) throw new Exception("El nombre de contacto es obligatorio");
            if (string.IsNullOrEmpty(Email)) throw new Exception("El email es obligatorio");
            if (string.IsNullOrEmpty(BankAccount)) throw new Exception("La cuenta bancaria es obligatoria");
        } 
        public void Update(UpdatableData data)
        {
            Name = data.Name;
            RUT = data.RUT;
            RazonSocial = data.RazonSocial;
            Address = data.Address;
            MapsAddress = data.MapsAddress;
            Phone = data.Phone;
            ContactName = data.ContactName;
            Email = data.Email;
            BankAccount = data.BankAccount;
            Observations = data.Observations;
            Validate();
        }
        public class UpdatableData
        {
            public string Name { get; set; }
            public string RUT { get; set; }
            public string RazonSocial { get; set; }
            public string Address { get; set; }
            public string MapsAddress { get; set; }
            public string Phone { get; set; }
            public string ContactName { get; set; }
            public string Email { get; set; }
            public string BankAccount { get; set; }
            public string Observations { get; set; }
        }
    }
}
