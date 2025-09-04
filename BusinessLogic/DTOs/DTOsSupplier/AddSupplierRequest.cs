using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsBankAccount;

namespace BusinessLogic.DTOs.DTOsSupplier
{
    public class AddSupplierRequest : AuditableRequest
    {
        public required string Name { get; set; }
        public required string RUT { get; set; }
        public required string RazonSocial { get; set; }
        public string? Address { get; set; }
        public Location? Location { get; set; }
        public required string Phone { get; set; }
        public required string ContactName { get; set; }
        public string? Email { get; set; }
        public string? Observations { get; set; }
        public List<BankAccountAddRequest>? BankAccounts { get; set; }
    }
}
