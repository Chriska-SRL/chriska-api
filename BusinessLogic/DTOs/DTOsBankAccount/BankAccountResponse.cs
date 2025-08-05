using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.DTOs.DTOsAudit;

namespace BusinessLogic.DTOs.DTOsBankAccount
{
    public class BankAccountResponse
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public Bank Bank { get; set; }
    }
}
