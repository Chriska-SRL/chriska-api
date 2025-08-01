using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class SupplierRepository : Repository<Supplier, Supplier.UpdatableData>, ISupplierRepository
    {
        public SupplierRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            int newId = await ExecuteWriteWithAuditAsync(
            "INSERT INTO Suppliers (Name, RUT, RazonSocial, Address, MapsAddress, Phone, ContactName, Email, Observations) " +
            "OUTPUT INSERTED.Id VALUES (@Name, @RUT, @RazonSocial, @Address, @MapsAddress, @Phone, @ContactName, @Email, @Observations)",
            supplier,
            AuditAction.Insert,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", supplier.Name);
                cmd.Parameters.AddWithValue("@RUT", supplier.RUT);
                cmd.Parameters.AddWithValue("@RazonSocial", supplier.RazonSocial);
                cmd.Parameters.AddWithValue("@Address", supplier.Address);
                cmd.Parameters.AddWithValue("@MapsAddress", supplier.MapsAddress);
                cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                cmd.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                cmd.Parameters.AddWithValue("@Email", supplier.Email);
                cmd.Parameters.AddWithValue("@Observations", supplier.Observations);
            },
            async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            await AddSupplierBankAccountsAsync(newId, supplier.BankAccounts);

            return new Supplier(newId, supplier.Name, supplier.RUT, supplier.RazonSocial, supplier.Address, supplier.MapsAddress, supplier.Phone,
                                supplier.ContactName, supplier.Email, supplier.Observations, supplier.BankAccounts, supplier.AuditInfo);
        }

        public async Task<Supplier> DeleteAsync(Supplier supplier)
        {
            await ExecuteWriteWithAuditAsync(
                @"UPDATE Suppliers SET IsDeleted = 1 WHERE Id = @Id",
                supplier,
                AuditAction.Delete,
                cmd => cmd.Parameters.AddWithValue("@Id", supplier.Id));

            await DeleteSupplierBankAccountsAsync(supplier.Id);
            return supplier;
        }

        public async Task<List<Supplier>> GetAllAsync(QueryOptions options)
        {
            var supplierDict = new Dictionary<int, Supplier>();

            var allowedFilters = new[] { "Name", "RUT", "RazonSocial", "Address", "Phone", "ContactName", "Email" };

            return await ExecuteReadAsync(
                baseQuery: @"SELECT s.*, b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber
                             FROM Suppliers s
                             LEFT JOIN SupplierBankAccounts b ON s.Id = b.SupplierId",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int supplierId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!supplierDict.TryGetValue(supplierId, out var supplier))
                        {
                            supplier = SupplierMapper.FromReader(reader);
                            supplier.BankAccounts = new List<BankAccount>();
                            supplierDict.Add(supplierId, supplier);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                        {
                            supplier.BankAccounts.Add(BankAccountMapper.FromReader(reader));
                        }
                    }

                    return supplierDict.Values.ToList();
                },
                options: options,
                tableAlias: "s",
                allowedFilterColumns: allowedFilters
            );
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            Supplier? result = null;

            await ExecuteReadAsync(
                baseQuery: @"SELECT s.*, b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber
                             FROM Suppliers s
                             LEFT JOIN SupplierBankAccounts b ON s.Id = b.SupplierId
                             WHERE s.Id = @Id",
                map: reader =>
                {
                    Supplier? supplier = null;
                    while (reader.Read())
                    {
                        if (supplier == null)
                        {
                            supplier = SupplierMapper.FromReader(reader);
                            supplier.BankAccounts = new List<BankAccount>();
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                            supplier.BankAccounts.Add(BankAccountMapper.FromReader(reader));
                    }
                    result = supplier;
                    return supplier;
                },
                options: new QueryOptions(),
                tableAlias: "s",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id));

            return result;
        }

        public Task<Supplier?> GetByNameAsync(string name)
        {
            return GetByFieldAsync("Name", name);
        }

        public Task<Supplier?> GetByRazonSocialAsync(string razonSocial)
        {
            return GetByFieldAsync("RazonSocial", razonSocial);
        }

        public Task<Supplier?> GetByRUTAsync(string rut)
        {
            return GetByFieldAsync("RUT", rut);
        }

        private Task<Supplier?> GetByFieldAsync(string fieldName, string value)
        {
            return ExecuteReadAsync(
                baseQuery: $@"SELECT s.*, b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber
                              FROM Suppliers s
                              LEFT JOIN SupplierBankAccounts b ON s.Id = b.SupplierId
                              WHERE s.{fieldName} = @Value",
                map: reader =>
                {
                    Supplier? supplier = null;
                    while (reader.Read())
                    {
                        if (supplier == null)
                        {
                            supplier = SupplierMapper.FromReader(reader);
                            supplier.BankAccounts = new List<BankAccount>();
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                            supplier.BankAccounts.Add(BankAccountMapper.FromReader(reader));
                    }
                    return supplier;
                },
                options: new QueryOptions(),
                tableAlias: "s",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Value", value));
        }
        public async Task<Supplier> UpdateAsync(Supplier supplier)
        {
            await ExecuteWriteWithAuditAsync(
                baseQuery: @"UPDATE Suppliers 
                            SET Name = @Name, RUT = @RUT, RazonSocial = @RazonSocial, Address = @Address, 
                                MapsAddress = @MapsAddress, Phone = @Phone, ContactName = @ContactName, 
                                Email = @Email, Observations = @Observations
                            WHERE Id = @Id",
                supplier,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", supplier.Id);
                    cmd.Parameters.AddWithValue("@Name", supplier.Name);
                    cmd.Parameters.AddWithValue("@RUT", supplier.RUT);
                    cmd.Parameters.AddWithValue("@RazonSocial", supplier.RazonSocial);
                    cmd.Parameters.AddWithValue("@Address", supplier.Address);
                    cmd.Parameters.AddWithValue("@MapsAddress", supplier.MapsAddress);
                    cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                    cmd.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                    cmd.Parameters.AddWithValue("@Email", supplier.Email);
                    cmd.Parameters.AddWithValue("@Observations", supplier.Observations);
                });

            await DeleteSupplierBankAccountsAsync(supplier.Id);
            await AddSupplierBankAccountsAsync(supplier.Id, supplier.BankAccounts);

            return supplier;
        }

        private async Task AddSupplierBankAccountsAsync(int supplierId, IEnumerable<BankAccount> bankAccounts)
        {
            if (bankAccounts == null || !bankAccounts.Any())
                return;

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var values = new List<string>();
                var parameters = new List<SqlParameter>();
                int i = 0;

                foreach (var account in bankAccounts)
                {
                    values.Add($"(@SupplierId, @BankName{i}, @AccountName{i}, @AccountNumber{i})");

                    parameters.Add(new SqlParameter($"@BankName{i}", account.Bank.ToString()));
                    parameters.Add(new SqlParameter($"@AccountName{i}", account.AccountName));
                    parameters.Add(new SqlParameter($"@AccountNumber{i}", account.AccountNumber));
                    i++;
                }

                string sql = $@"INSERT INTO SupplierBankAccounts (SupplierId, BankName, AccountName, AccountNumber)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar cuentas bancarias del proveedor.", ex);
            }
        }

        private async Task DeleteSupplierBankAccountsAsync(int supplierId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"DELETE FROM SupplierBankAccounts WHERE SupplierId = @SupplierId";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar cuentas bancarias del proveedor.", ex);
            }
        }
    }
}
