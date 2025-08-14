using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Common.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;
using System.Data;

namespace Repository.EntityRepositories
{
    public class ReturnRequestRepository : Repository<ReturnRequest, ReturnRequest.UpdatableData>, IReturnRequestRepository
    {
        public ReturnRequestRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<ReturnRequest> AddAsync(ReturnRequest request)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO ReturnRequests (ClientId, Status, ConfirmedDate, Date, Observations, UserId, DeliveryId) 
                  VALUES (@ClientId, @Status, @ConfirmedDate, @Date, @Observations, @UserId, @DeliveryId);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                request,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@ClientId", request.Client.Id);
                    cmd.Parameters.AddWithValue("@Status", request.Status.ToString());
                    cmd.Parameters.AddWithValue("@ConfirmedDate", (object?)request.ConfirmedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Date", request.Date);
                    cmd.Parameters.AddWithValue("@Observations", request.Observations ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserId", request.User?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DeliveryId", request.Delivery.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            request.Id = newId;
            return request;
        }

        #endregion

        #region Update

        public async Task<ReturnRequest> UpdateAsync(ReturnRequest request)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE ReturnRequests 
                  SET ClientId = @ClientId, Status = @Status, ConfirmedDate = @ConfirmedDate, Date = @Date,
                      Observations = @Observations, UserId = @UserId, DeliveryId = @DeliveryId
                  WHERE Id = @Id",
                request,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", request.Id);
                    cmd.Parameters.AddWithValue("@ClientId", request.Client.Id);
                    cmd.Parameters.AddWithValue("@Status", request.Status.ToString());
                    cmd.Parameters.AddWithValue("@ConfirmedDate", (object?)request.ConfirmedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Date", request.Date);
                    cmd.Parameters.AddWithValue("@Observations", request.Observations ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserId", request.User?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DeliveryId", request.Delivery.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la devolución con Id {request.Id}");

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

        #region GetAll

        public async Task<List<ReturnRequest>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "ClientId", "Status", "Date" };
            return await ExecuteReadAsync(
                baseQuery: @"SELECT rr.*, 
                                    c.Id AS ClientId, c.Name AS ClientName, 
                                    u.Id AS UserId, u.Username AS UserUsername, 
                                    d.Id AS DeliveryId, d.Address AS DeliveryAddress
                             FROM ReturnRequests rr
                             INNER JOIN Clients c ON rr.ClientId = c.Id
                             LEFT JOIN Users u ON rr.UserId = u.Id
                             INNER JOIN Deliveries d ON rr.DeliveryId = d.Id",
                map: reader =>
                {
                    var requests = new List<ReturnRequest>();

                    while (reader.Read())
                    {
                        var returnRequest = ReturnRequestMapper.FromReader(reader);
                        requests.Add(returnRequest);
                    }

                    return requests;
                },
                options: options,
                allowedFilterColumns: allowedFilters,
                tableAlias: "rr"
            );
        }

        #endregion

        #region GetById

        public async Task<ReturnRequest?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"SELECT rr.*, 
                                    c.Id AS ClientId, c.Name AS ClientName, 
                                    u.Id AS UserId, u.Username AS UserUsername, 
                                    d.Id AS DeliveryId, d.Address AS DeliveryAddress
                             FROM ReturnRequests rr
                             INNER JOIN Clients c ON rr.ClientId = c.Id
                             LEFT JOIN Users u ON rr.UserId = u.Id
                             INNER JOIN Deliveries d ON rr.DeliveryId = d.Id
                             WHERE rr.Id = @Id",
                map: reader =>
                {
                    ReturnRequest? returnRequest = null;
                    if (reader.Read())
                    {
                        returnRequest = ReturnRequestMapper.FromReader(reader);
                    }
                    return returnRequest;
                },
                options: new QueryOptions(),
                tableAlias: "rr",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion
    }
}
