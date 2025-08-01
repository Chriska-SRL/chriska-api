using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsBankAccount
{
    public class BankAccountAddRequest
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public Bank Bank { get; set; }
    }
}
