using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;
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

            return new Client(newId, client.Name, client.RUT, client.RazonSocial, client.Address, client.MapsAddress,
                              client.Schedule, client.Phone, client.ContactName, client.Email, client.Observations,
                              client.BankAccounts, client.LoanedCrates, client.Qualification, client.Zone, client.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Client> UpdateAsync(Client client)
        {
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
            var clientDict = new Dictionary<int, Client>();

            return await ExecuteReadAsync(
                baseQuery: @"SELECT 
                        c.*, 
                        z.Id AS ZoneId, z.Name AS ZoneName,
                        b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber,
                        b.CreatedAt, b.CreatedBy, b.CreatedLocation,
                        b.UpdatedAt, b.UpdatedBy, b.UpdatedLocation,
                        b.DeletedAt, b.DeletedBy, b.DeletedLocation
                     FROM Clients c
                     INNER JOIN Zones z ON c.ZoneId = z.Id
                     LEFT JOIN ClientBankAccounts b ON c.Id = b.ClientId AND b.IsDeleted = 0
                     WHERE c.IsDeleted = 0",
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
                            client.BankAccounts.Add(BankAccountMapper.FromReaderWithClientJoin(reader));
                        }
                    }

                    return clientDict.Values.ToList();
                },
                options: options
            );
        }


        #endregion

        #region GetById

        public async Task<Client?> GetByIdAsync(int id)
        {
            Client? client = null;

            await ExecuteReadAsync(
                baseQuery: @"SELECT 
                        c.*, 
                        z.Id AS ZoneId, z.Name AS ZoneName,
                        b.Id AS BankAccountId, b.BankName, b.AccountName, b.AccountNumber,
                        b.CreatedAt, b.CreatedBy, b.CreatedLocation,
                        b.UpdatedAt, b.UpdatedBy, b.UpdatedLocation,
                        b.DeletedAt, b.DeletedBy, b.DeletedLocation
                     FROM Clients c
                     INNER JOIN Zones z ON c.ZoneId = z.Id
                     LEFT JOIN ClientBankAccounts b ON c.Id = b.ClientId AND b.IsDeleted = 0
                     WHERE c.Id = @Id AND c.IsDeleted = 0",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        if (client == null)
                        {
                            client = ClientMapper.FromReader(reader);
                            client.BankAccounts = new List<BankAccount>();
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                        {
                            client.BankAccounts.Add(BankAccountMapper.FromReaderWithClientJoin(reader));
                        }
                    }

                    return client;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );

            return client;
        }


        #endregion
    }
}
