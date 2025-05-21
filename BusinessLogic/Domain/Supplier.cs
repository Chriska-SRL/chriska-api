using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUT { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string BankAccount { get; set; }
        public string Observation { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public List<Day> DaysToDeliver { get; set; } = new List<Day>();

        public Supplier(string name, string rUT, string businessName, string address, string mapsAddress, string phone, string contactName, string email, string bankAccount, string observation)
        {
            Name = name;
            RUT = rUT;
            BusinessName = businessName;
            Address = address;
            MapsAddress = mapsAddress;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            BankAccount = bankAccount;
            Observation = observation;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new Exception("El nombre es obligatorio");
            if (string.IsNullOrEmpty(RUT)) throw new Exception("El RUT es obligatorio");
            if (string.IsNullOrEmpty(BusinessName)) throw new Exception("La razon social es obligatoria");
            if (string.IsNullOrEmpty(Address)) throw new Exception("La direccion es obligatoria");
            if (string.IsNullOrEmpty(Phone)) throw new Exception("El telefono es obligatorio");
            if (string.IsNullOrEmpty(ContactName)) throw new Exception("El nombre de contacto es obligatorio");
            if (string.IsNullOrEmpty(Email)) throw new Exception("El email es obligatorio");
            if (string.IsNullOrEmpty(BankAccount)) throw new Exception("La cuenta bancaria es obligatoria");
        }
        public void Update(string name, string rUT, string businessName, string address, string mapsAddress, string phone, string contactName, string email, string bankAccount, string observation)
        {
            Name = name;
            RUT = rUT;
            BusinessName = businessName;
            Address = address;
            MapsAddress = mapsAddress;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            BankAccount = bankAccount;
            Observation = observation;
        }

    }
}
