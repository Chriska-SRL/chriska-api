using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
