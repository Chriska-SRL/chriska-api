using BusinessLogic.Domain;
using BusinessLogic.Común.Enums;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class BankAccountMapper
    {
        public static BankAccount FromReader(SqlDataReader reader)
        {
            string bankString = reader["BankName"]?.ToString()
                ?? throw new InvalidOperationException("El campo 'BankName' no puede ser nulo.");

            if (!Enum.TryParse<Bank>(bankString, out var bank))
                throw new InvalidOperationException($"Valor inválido para el banco: '{bankString}'");

            return new BankAccount(
                accountName: reader.GetString(reader.GetOrdinal("AccountName")),
                accountNumber: reader.GetString(reader.GetOrdinal("AccountNumber")),
                bank: bank
            );
        }
    }
}
