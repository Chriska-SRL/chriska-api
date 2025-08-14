using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ReturnRequestRepository : Repository<ReturnRequest, ReturnRequest.UpdatableData>, IReturnRequestRepository
    {
        public ReturnRequestRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }
        #region Query (base)
        private string baseQuery = @"
        SELECT
    -- ReturnRequest
    rr.*,

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

    -- Delivery
    d.Id              AS DeliveryId,
    d.ConfirmedDate   AS DeliveryConfirmedDate,
    d.Crates          AS DeliveryCrates,
    d.Status          AS DeliveryStatus,
    d.Date            AS DeliveryDate,
    d.Observations    AS DeliveryObservations,
    d.OrderId AS OrderId,

    --Order
    o.Id             AS OrderId,
    o.Date           AS OrderDate,
    o.ConfirmedDate  AS OrderConfirmedDate,
    o.Status         AS OrderStatus,
    o.Crates         AS OrderCrates,
    o.Observations   AS OrderObservations
    

FROM ReturnRequests rr
LEFT JOIN Clients c  ON c.Id = rr.ClientId
LEFT JOIN Zones z    ON z.Id = c.ZoneId
LEFT JOIN Users u    ON u.Id = rr.CreatedBy
LEFT JOIN Roles r    ON r.Id = u.RoleId
LEFT JOIN Deliveries d ON d.Id = rr.DeliveryId
LEFT JOIN Orders o ON o.Id = d.OrderId
    ";
        #endregion
        #region Add

        public async Task<ReturnRequest> AddAsync(ReturnRequest request)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO ReturnRequests (ClientId, Date, Observations, DeliveryId, Status)
              OUTPUT INSERTED.Id VALUES (@ClientId, @Date, @Observations, @DeliveryId, @Status)",
                request,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@ClientId", request.Client.Id);
                    cmd.Parameters.AddWithValue("@Date", request.Date);
                    cmd.Parameters.AddWithValue("@Observations", request.Observations ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DeliveryId", request.Delivery?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", request.Status.ToString());
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            request.Id = newId;
            return request;
        }
        #endregion


        #region Delete

        public async Task<ReturnRequest> DeleteAsync(ReturnRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "La solicitud no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE ReturnRequests SET IsDeleted = 1 WHERE Id = @Id",
                request,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", request.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la devolución con Id {request.Id}");

            return request;
        }

        #endregion

        public async Task<List<ReturnRequest>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "ClientId", "Status", "Date" };
            var dict = new Dictionary<int, ReturnRequest>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        if (!dict.TryGetValue(id, out var rr))
                        {
                            rr = ReturnRequestMapper.FromReader(reader);
                            dict.Add(id, rr);
                        }
                    }
                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "rr",
                allowedFilterColumns: allowedFilters
            );
        }

        #region GetById

        public async Task<ReturnRequest?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE rr.Id = @Id",
                map: reader =>
                {
                    ReturnRequest? request = null;
                    while (reader.Read())
                    {
                        if (request == null)
                            request = ReturnRequestMapper.FromReader(reader);
                    }
                    return request;
                },
                options: new QueryOptions(),
                tableAlias: "rr",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }
        #endregion
        public Task<ReturnRequest> UpdateAsync(ReturnRequest entity)
        {
            throw new NotImplementedException();
        }

    }
}
