using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.NewFolder
{
    public class AddClientRequest
    {
        private string Name { get; set; }
        private string RUT { get; set; }
        private string RazonSocial { get; set; }
        private string Address { get; set; }
        private string MapsAddress { get; set; }
        private string Phone { get; set; }
        private string Schedule { get; set; }
        private string Email { get; set; }
        private string ContactName { get; set; }
        private string BankAccount { get; set; }
        private int LoanedCrates { get; set; }
        private string Observations { get; set; }

    }
}
