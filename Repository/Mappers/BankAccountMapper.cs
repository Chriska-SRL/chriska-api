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
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                accountName: reader.GetString(reader.GetOrdinal("AccountName")),
                accountNumber: reader.GetString(reader.GetOrdinal("AccountNumber")),
                bank: bank,
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }
        public static BankAccount FromReaderWithClientJoin(SqlDataReader reader)
        {
            // Si no hay BankAccountId, no hay cuenta en esta fila
            if (reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                throw new InvalidOperationException("No hay datos de cuenta bancaria en esta fila.");

            string bankString = reader["BankName"]?.ToString()
                ?? throw new InvalidOperationException("El campo 'BankName' no puede ser nulo.");

            if (!Enum.TryParse<Bank>(bankString, out var bank))
                throw new InvalidOperationException($"Valor inválido para el banco: '{bankString}'");

            return new BankAccount(
                id: reader.GetInt32(reader.GetOrdinal("BankAccountId")),
                accountName: reader.GetString(reader.GetOrdinal("AccountName")),
                accountNumber: reader.GetString(reader.GetOrdinal("AccountNumber")),
                bank: bank,
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }
    }
}
