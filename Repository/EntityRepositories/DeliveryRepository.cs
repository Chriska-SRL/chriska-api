using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class DeliveryRepository : Repository<Delivery, Delivery.UpdatableData>, IDeliveryRepository
    {
        public DeliveryRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        private string baseQuery = @"
        SELECT
            -- Delivery
            d.*,

            -- Client
            c.Id              AS ClientId,
            c.Name            AS ClientName,
            c.RUT             AS ClientRUT,
            c.RazonSocial     AS ClientRazonSocial,
            c.Address         AS ClientAddress,
            c.MapsAddress     AS ClientMapsAddress,
            c.Schedule        AS ClientSchedule,
            c.Phone           AS ClientPhone,
            c.ContactName     AS ClientContactName,
            c.Email           AS ClientEmail,
            c.Observations    AS ClientObservations,
            c.LoanedCrates    AS ClientLoanedCrates,
            c.Qualification   AS ClientQualification,
            c.ZoneId          AS ZoneId,

            -- Zone
            z.Name            AS ZoneName,
            z.Description     AS ZoneDescription,
            z.DeliveryDays    AS ZoneDeliveryDays,
            z.RequestDays     AS ZoneRequestDays,
            z.ImageUrl        AS ZoneImageUrl,

            -- User
            u.Id              AS UserId,
            u.Name            AS UserName,
            u.Username        AS UserUsername,
            u.Password        AS UserPassword,
            u.IsEnabled       AS UserIsEnabled,
            u.NeedsPasswordChange AS UserNeedsPasswordChange,

            -- Role
            r.Id              AS UserRoleId,
            r.Name            AS UserRoleName,
            r.Description     AS UserRoleDescription,

            -- Order
            o.Id              AS OrderId,
            o.Date            AS OrderDate,
            o.ConfirmedDate   AS OrderConfirmedDate,
            o.Status          AS OrderStatus,
            o.Observations    AS OrderObservations,
            o.Crates        AS OrderCrates

        FROM Deliveries d
        LEFT JOIN Clients c  ON c.Id = d.ClientId
        LEFT JOIN Zones z    ON z.Id = c.ZoneId
        LEFT JOIN Users u    ON u.Id = d.CreatedBy
        LEFT JOIN Roles r    ON r.Id = u.RoleId
        LEFT JOIN Orders o   ON o.Id = d.OrderId
    ";

        #region Add

        public async Task<Delivery> AddAsync(Delivery delivery)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO Deliveries ( Id,Observations, Crates, Status,OrderId, Date,ClientId)
                  OUTPUT INSERTED.Id VALUES (@Id ,@Observations, @Crates, @Status, @OrderId,@Date,@ClientId)",
                delivery,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", delivery.Order.Id);
                    cmd.Parameters.AddWithValue("@Observations", delivery.Observations ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Crates", delivery.Crates);
                    cmd.Parameters.AddWithValue("@Status", delivery.Status.ToString());
                    cmd.Parameters.AddWithValue("@OrderId", delivery.Order?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Date", delivery.Date);
                    cmd.Parameters.AddWithValue("@ClientId", delivery.Client?.Id ?? throw new Exception("Cliente nulo"));


                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            delivery.Id = newId;
            return delivery;
        }

        #endregion

        #region Delete

        public async Task<Delivery> DeleteAsync(Delivery delivery)
        {
            if (delivery == null)
                throw new ArgumentNullException(nameof(delivery), "La entrega no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Deliveries SET IsDeleted = 1 WHERE Id = @Id",
                delivery,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", delivery.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la entrega con Id {delivery.Id}");

            return delivery;
        }

        #endregion

        public async Task<List<Delivery>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "ClientId", "Status", "Date" };
            var dict = new Dictionary<int, Delivery>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!dict.TryGetValue(id, out var delivery))
                        {
                            delivery = DeliveryMapper.FromReader(reader);
                            dict.Add(id, delivery);
                        }
                    }
                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "d",
                allowedFilterColumns: allowedFilters
            );
        }

        #region GetById

        public async Task<Delivery?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE d.Id = @Id",
                map: reader =>
                {
                    Delivery? delivery = null;
                    while (reader.Read())
                    {
                        if (delivery == null)
                            delivery = DeliveryMapper.FromReader(reader);
                    }
                    return delivery;
                },
                options: new QueryOptions(),
                tableAlias: "d",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion

        public Task<Delivery> UpdateAsync(Delivery entity)
        {
            throw new NotImplementedException();
        }
    }
}
