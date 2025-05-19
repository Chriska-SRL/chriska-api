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

    }
}
