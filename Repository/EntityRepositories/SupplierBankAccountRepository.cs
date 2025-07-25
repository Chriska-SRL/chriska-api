using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class SupplierBankAccountRepository : Repository<BankAccount, BankAccount.UpdatableData>, IBankAccountRepository
    {
        public SupplierBankAccountRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public async Task<BankAccount> AddAsync(BankAccount entity)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO SupplierBankAccounts (BankName, AccountName, AccountNumber) OUTPUT INSERTED.Id VALUES (@BankName, @AccountName, @AccountNumber)",
                entity,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@BankName", (object?)entity.Bank.ToString() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccountName", entity.AccountName);
                    cmd.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            return new BankAccount(newId, entity.AccountName, entity.AccountNumber, entity.Bank, entity.AuditInfo);
        }

        public async Task<BankAccount> UpdateAsync(BankAccount entity)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE SupplierBankAccounts SET BankName = @BankName, AccountName = @AccountName, AccountNumber = @AccountNumber WHERE Id = @Id",
                entity,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@BankName", (object?)entity.Bank.ToString() ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccountName", entity.AccountName);
                    cmd.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la cuenta bancaria del proveedor con Id {entity.Id}");

            return entity;
        }

        public async Task<BankAccount> DeleteAsync(BankAccount entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE SupplierBankAccounts SET IsDeleted = 1 WHERE Id = @Id",
                entity,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la cuenta bancaria del proveedor con Id {entity.Id}");

            return entity;
        }

        public async Task<List<BankAccount>> GetAllAsync(QueryOptions options)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM SupplierBankAccounts",
                map: reader =>
                {
                    var list = new List<BankAccount>();
                    while (reader.Read())
                        list.Add(BankAccountMapper.FromReader(reader));
                    return list;
                },
                options: options
            );
        }

        public async Task<BankAccount?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM SupplierBankAccounts WHERE Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return BankAccountMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }
    }
}
