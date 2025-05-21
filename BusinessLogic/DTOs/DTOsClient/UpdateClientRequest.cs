using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsClient
{
    public class UpdateClientRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RUT { get; set; }
        public string RazonSocial { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string Schedule { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string BankAccount { get; set; }
        public string Observations { get; set; }
        public int LoanedCrates { get; set; }
        public int ZoneId { get; set; }
    }
}
