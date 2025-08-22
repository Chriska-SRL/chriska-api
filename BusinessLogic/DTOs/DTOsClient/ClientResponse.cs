using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBankAccount;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.DTOs.DTOsClient
{
    public class ClientResponse : AuditableResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RazonSocial { get; set; }
        public string RUT { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string Schedule { get; set; }
        public int LoanedCrates { get; set; }
        public string Observations { get; set; }
        public ZoneResponse Zone { get; set; } 
        public List<BankAccountResponse> BankAccounts { get; set; } = new List<BankAccountResponse>();
        public string Qualification { get; set; } 
    }
}
