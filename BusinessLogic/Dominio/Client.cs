using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Client
    {
        private int ClientId { get; set; }
        private string Name { get; set; }
        private string RUT { get; set; }
        private string RazonSocial { get; set; }
        private string Address { get; set; }
        private string MapsAddress { get; set; }
        private string Schedule { get; set; }
        private string Phone { get; set; }
        private string ContactName { get; set; }
        private string Email { get; set; }
        private string Observation { get; set; }
        private string BankAccount { get; set; }
        private int LoanedCrates { get; set; }
        private List<Request> Requests { get; set; } = new List<Request>();
        private Zone Zone { get; set; }
        private List<Receipt> Receipts { get; set; } = new List<Receipt>();
    }
}
