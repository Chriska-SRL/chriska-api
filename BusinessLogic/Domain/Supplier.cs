using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Supplier
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string RUT { get; set; }
        private string BusinessName { get; set; }
        private string Address { get; set; }
        private string MapsAddress { get; set; }
        private string Phone { get; set; }
        private string ContactName { get; set; }
        private string Email { get; set; }
        private string BankAccount { get; set; }
        private string Observation { get; set; }
        private List<Product> Products { get; set; } = new List<Product>();
        private List<Payment> Payments { get; set; } = new List<Payment>();
        private List<Purchase> Purchases { get; set; } = new List<Purchase>();
        private List<Day> DaysToDeliver { get; set; } = new List<Day>();

    }
}
