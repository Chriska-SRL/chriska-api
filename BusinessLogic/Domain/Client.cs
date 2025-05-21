using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Client
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

        public Client(string name, string rut,string razonSocial, string address, string mapsAddress, string schedule, string phone,string contactName,string email,string observation,string bankAccount, int loanedCrates,Zone zone)
        {
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

        public void Update(string name, string rut, string razonSocial,string address, string mapsAddress,string stringSchedule,string phone,string contactName,string email, string observation, string bankAccount, int loanedCrates,Zone zone)
        {

            Name = name;
            RUT = rut;
            RazonSocial = razonSocial;
            Address = address;
            MapsAddress = mapsAddress;
            Schedule = stringSchedule;
            Phone = phone;
            ContactName = contactName;
            Email = email;
            Observation = observation;
            BankAccount = bankAccount;
            LoanedCrates = loanedCrates;
            Zone = zone;

        }
    }
}
