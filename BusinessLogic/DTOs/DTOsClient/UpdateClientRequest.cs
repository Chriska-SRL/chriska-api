using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBankAccount;

namespace BusinessLogic.DTOs.DTOsClient
{
    public class UpdateClientRequest : AuditableRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RazonSocial { get; set; }
        public string? RUT { get; set; }
        public string? Address { get; set; }
        public string? MapsAddress { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactName { get; set; }
        public string? Schedule { get; set; }
        public int? LoanedCrates { get; set; }
        public string? Observations { get; set; }
        public int? ZoneId { get; set; }
        public List<BankAccountAddRequest>? BankAccounts { get; set; }
        public string? Qualification { get; set; } 
    }
}
