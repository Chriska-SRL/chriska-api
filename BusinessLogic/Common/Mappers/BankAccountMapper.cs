using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsBankAccount;

namespace BusinessLogic.Common.Mappers
{
    public class BankAccountMapper
    {
        public static BankAccount ToDomain(BankAccountAddRequest request)
        {
            return new BankAccount(request.AccountName, request.AccountNumber, request.Bank);

        }

        public static BankAccountResponse ToResponse(BankAccount bankAccount)
        {
            return new BankAccountResponse
            {
                AccountName = bankAccount.AccountName,
                AccountNumber = bankAccount.AccountNumber,
                Bank = bankAccount.Bank,
            };
        }
    }
}
