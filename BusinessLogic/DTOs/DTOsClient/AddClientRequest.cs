using BusinessLogic.Común.Audits;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBankAccount;

namespace BusinessLogic.DTOs.DTOsClient
{
    public class AddClientRequest : AuditableRequest
    {
        public string Name { get; set; }
        public string RazonSocial { get; set; }
        public string? RUT { get; set; }
        public string Address { get; set; }
        public string MapsAddress { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string ContactName { get; set; }
        public string Schedule { get; set; }
        public string? Observations { get; set; }
        public int ZoneId { get; set; }
        public List<BankAccountAddRequest> BankAccounts { get; set; }
    }
}
