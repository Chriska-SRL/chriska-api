using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ClientRepository : Repository<Client, Client.UpdatableData>, IClientRepository
    {
        public ClientRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Client> AddAsync(Client client)
        {
            int newId = await ExecuteWriteWithAuditAsync(
            "INSERT INTO Clients (Name, RUT, RazonSocial, Address, MapsAddress, Schedule, Phone, ContactName, Email, Observations, LoanedCrates, Qualification, ZoneId) " +
            "OUTPUT INSERTED.Id VALUES (@Name, @RUT, @RazonSocial, @Address, @MapsAddress, @Schedule, @Phone, @ContactName, @Email, @Observations, @LoanedCrates, @Qualification, @ZoneId)",
            client,
            AuditAction.Insert,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", client.Name);
                cmd.Parameters.AddWithValue("@RUT", client.RUT);
                cmd.Parameters.AddWithValue("@RazonSocial", client.RazonSocial);
                cmd.Parameters.AddWithValue("@Address", client.Address);
                cmd.Parameters.AddWithValue("@MapsAddress", client.MapsAddress);
                cmd.Parameters.AddWithValue("@Schedule", client.Schedule);
                cmd.Parameters.AddWithValue("@Phone", client.Phone);
                cmd.Parameters.AddWithValue("@ContactName", client.ContactName);
                cmd.Parameters.AddWithValue("@Email", client.Email);
                cmd.Parameters.AddWithValue("@Observations", client.Observations);
                cmd.Parameters.AddWithValue("@LoanedCrates", client.LoanedCrates);
                cmd.Parameters.AddWithValue("@Qualification", client.Qualification);
                cmd.Parameters.AddWithValue("@ZoneId", client.Zone.Id);
            },
            async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
        );

            await AddClientBankAccountsAsync(newId, client.BankAccounts);

            return new Client(newId, client.Name, client.RUT, client.RazonSocial, client.Address, client.MapsAddress,
                              client.Schedule, client.Phone, client.ContactName, client.Email, client.Observations,
                              client.BankAccounts, client.LoanedCrates, client.Qualification, client.Zone, client.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Client> UpdateAsync(Client client)
        {
            await DeleteClientBankAccountsAsync(client.Id);
            await AddClientBankAccountsAsync(client.Id, client.BankAccounts);

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Clients SET Name = @Name, RUT = @RUT, RazonSocial = @RazonSocial, Address = @Address, MapsAddress = @MapsAddress, " +
                "Schedule = @Schedule, Phone = @Phone, ContactName = @ContactName, Email = @Email, Observations = @Observations, " +
                "LoanedCrates = @LoanedCrates, Qualification = @Qualification, ZoneId = @ZoneId WHERE Id = @Id",
                client,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", client.Id);
                    cmd.Parameters.AddWithValue("@Name", client.Name);
                    cmd.Parameters.AddWithValue("@RUT", client.RUT);
                    cmd.Parameters.AddWithValue("@RazonSocial", client.RazonSocial);
                    cmd.Parameters.AddWithValue("@Address", client.Address);
                    cmd.Parameters.AddWithValue("@MapsAddress", client.MapsAddress);
                    cmd.Parameters.AddWithValue("@Schedule", client.Schedule);
                    cmd.Parameters.AddWithValue("@Phone", client.Phone);
                    cmd.Parameters.AddWithValue("@ContactName", client.ContactName);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Observations", client.Observations);
                    cmd.Parameters.AddWithValue("@LoanedCrates", client.LoanedCrates);
                    cmd.Parameters.AddWithValue("@Qualification", client.Qualification);
                    cmd.Parameters.AddWithValue("@ZoneId", client.Zone.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el cliente con Id {client.Id}");

            return client;
        }

        #endregion

        #region Delete

        public async Task<Client> DeleteAsync(Client client)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Clients SET IsDeleted = 1 WHERE Id = @Id",
                client,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", client.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el cliente con Id {client.Id}");

            return client;
        }

        #endregion

        #region GetAll

        public async Task<List<Client>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Name", "RUT", "RazonSocial", "Address", "Phone", "ContactName", "Email", "Schedule", "Qualification", "ZoneId" };
            var clientDict = new Dictionary<int, Client>();

            return await ExecuteReadAsync(
                baseQuery: @"SELECT 
                        c.*, 
                        z.Name AS ZoneName, z.Description AS ZoneDescription, z.DeliveryDays As ZoneDeliveryDays, z.RequestDays AS ZoneRequestDays, z.ImageUrl AS ZoneImageUrl,
                        b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber
                     FROM Clients c
                     INNER JOIN Zones z ON c.ZoneId = z.Id
                     LEFT JOIN ClientBankAccounts b ON c.Id = b.ClientId",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int clientId = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!clientDict.TryGetValue(clientId, out var client))
                        {
                            client = ClientMapper.FromReader(reader);
                            client.BankAccounts = new List<BankAccount>();
                            clientDict.Add(clientId, client);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                        {
                            client.BankAccounts.Add(BankAccountMapper.FromReader(reader));
                        }
                    }

                    return clientDict.Values.ToList();
                },
                options: options,
                tableAlias: "c",
                allowedFilterColumns: allowedFilters
            );
        }


        #endregion

        #region GetById

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await GetByFieldAsync("Id", id.ToString());
        }


        #endregion

        private async Task AddClientBankAccountsAsync(int clientId, IEnumerable<BankAccount> bankAccounts)
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
                    values.Add($"(@ClientId, @BankName{i}, @AccountName{i}, @AccountNumber{i})");

                    parameters.Add(new SqlParameter($"@BankName{i}", account.Bank.ToString()));
                    parameters.Add(new SqlParameter($"@AccountName{i}", account.AccountName));
                    parameters.Add(new SqlParameter($"@AccountNumber{i}", account.AccountNumber));
                    i++;
                }

                string sql = $@"INSERT INTO ClientBankAccounts (ClientId, BankName, AccountName, AccountNumber)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar cuentas bancarias del cliente.", ex);
            }
        }

        private async Task DeleteClientBankAccountsAsync(int clientId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"DELETE FROM ClientBankAccounts WHERE ClientId = @ClientId";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ClientId", clientId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar cuentas bancarias del cliente.", ex);
            }
        }

        public async Task<Client?> GetByNameAsync(string name)
        {
            return await GetByFieldAsync("Name", name);
        }

        public async Task<Client?> GetByRUTAsync(string rut)
        {
            return await GetByFieldAsync("RUT", rut);
        }

        private async Task<Client?> GetByFieldAsync(string fieldName, string value)
        {
            return await ExecuteReadAsync(
                baseQuery: $@"SELECT 
                            c.*, 
                            z.Name AS ZoneName, z.Description AS ZoneDescription, z.DeliveryDays As ZoneDeliveryDays, z.RequestDays AS ZoneRequestDays, z.ImageUrl AS ZoneImageUrl,
                            b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber
                         FROM Clients c
                         INNER JOIN Zones z ON c.ZoneId = z.Id
                         LEFT JOIN ClientBankAccounts b ON c.Id = b.ClientId
                         WHERE c.{fieldName} = @Value",
                map: reader =>
                {
                    Client? client = null;
                    while (reader.Read())
                    {
                        if (client == null)
                        {
                            client = ClientMapper.FromReader(reader);
                            client.BankAccounts = new List<BankAccount>();
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                            client.BankAccounts.Add(BankAccountMapper.FromReader(reader));
                    }
                    return client;
                },
                options: new QueryOptions(),
                tableAlias: "c",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Value", value));
        }
    }
}
