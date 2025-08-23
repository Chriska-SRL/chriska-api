using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
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
    c.Location     AS ClientLocation,
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
    o.Observations   AS OrderObservations,

            -- Items (prefijo RR_)
                rrp.ReturnRequestId     AS RR_ReturnRequestId,
                rrp.Quantity       AS RR_Quantity,
                rrp.UnitPrice      AS RR_UnitPrice,
                rrp.Discount       AS RR_Discount,
                rrp.Weight AS RR_Weight,

            -- Product (prefijo P_)
                p.Id               AS ProductId,
                p.Name             AS ProductName,
                p.Description      AS ProductDescription,
                p.InternalCode     AS ProductInternalCode,
                p.Barcode          AS ProductBarcode,
                p.UnitType         AS ProductUnitType,
                p.Price            AS ProductPrice,
                p.TemperatureCondition AS ProductTemperatureCondition,
                p.EstimatedWeight  AS ProductEstimatedWeight,
                p.Stock            AS ProductStock,
                p.AvailableStock   AS ProductAvailableStock,
                p.Observations     AS ProductObservations,
                p.ImageUrl         AS ProductImageUrl,
                p.SubCategoryId    AS SubCategoryId,
                p.BrandId          AS BrandId,
                p.ShelveId         AS ProductShelveId,

                -- Brand (prefijo Brand)
                b.Name             AS BrandName,
                b.Description      AS BrandDescription,

                -- SubCategory (prefijo SubCategory)
                sb.Name            AS SubCategoryName,
                sb.Description     AS SubCategoryDescription,
                sb.CategoryId      AS SubCategoryCategoryId,

                -- Category (prefijo Category)
                cat.Name           AS CategoryName,
                cat.Description    AS CategoryDescription
    

        FROM ReturnRequests rr
        LEFT JOIN Clients c  ON c.Id = rr.ClientId
        LEFT JOIN Zones z    ON z.Id = c.ZoneId
        LEFT JOIN Users u    ON u.Id = rr.CreatedBy
        LEFT JOIN Roles r    ON r.Id = u.RoleId
        LEFT JOIN Deliveries d ON d.Id = rr.DeliveryId
        LEFT JOIN Orders o ON o.Id = d.OrderId
        LEFT JOIN ReturnRequests_Products rrp ON rrp.ReturnRequestId = rr.Id
        LEFT JOIN Products p ON rrp.ProductId = p.Id  
        LEFT JOIN Brands b        ON b.Id = p.BrandId
        LEFT JOIN SubCategories sb ON sb.Id = p.SubCategoryId
        LEFT JOIN Categories cat   ON cat.Id = sb.CategoryId

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

            await AddProductItems(newId, request.ProductItems);
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

                        var item = ProductItemMapper.FromReader(reader, "RR_");
                        if (item is not null) rr!.ProductItems.Add(item);

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
            var dict = new Dictionary<int, ReturnRequest>();


            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE rr.Id = @Id",
                map: reader =>
                {

                    while (reader.Read())
                    {
                        int rrId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(rrId, out var request))
                        {
                            request = ReturnRequestMapper.FromReader(reader);
                            dict.Add(rrId, request);
                        }

                        var item = ProductItemMapper.FromReader(reader, "RR_");
                        if (item is not null) request!.ProductItems.Add(item);
                    }

                    return dict.Values.FirstOrDefault();
                },
                options: new QueryOptions(),
                tableAlias: "rr",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion

        #region Update

        public async Task<ReturnRequest> UpdateAsync(ReturnRequest returnRequest)
        {
            await DeleteProductItems(returnRequest.Id);
            await AddProductItems(returnRequest.Id, returnRequest.ProductItems);

            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE ReturnRequests SET 
                        Observations = @Observations
                  WHERE Id = @Id",
                returnRequest,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", returnRequest.Id);
                    cmd.Parameters.AddWithValue("@Observations", (object?)returnRequest.Observations ?? DBNull.Value);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la solicitud con Id {returnRequest.Id}");

            return returnRequest;
        }

        #endregion


        #region Private helpers (items)

        private async Task AddProductItems(int returnRequestId, List<ProductItem> productItems)
        {
            if (productItems == null || !productItems.Any())
                return;

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var values = new List<string>();
                var parameters = new List<SqlParameter>();
                int i = 0;

                foreach (var productItem in productItems)
                {
                    values.Add($"(@ReturnRequestId, @Quantity{i}, @UnitPrice{i}, @Discount{i}, @ProductId{i})");

                    parameters.Add(new SqlParameter($"@Quantity{i}", productItem.Quantity));
                    parameters.Add(new SqlParameter($"@UnitPrice{i}", productItem.UnitPrice));
                    parameters.Add(new SqlParameter($"@Discount{i}", productItem.Discount));
                    parameters.Add(new SqlParameter($"@ProductId{i}", productItem.Product.Id));
                    i++;
                }

                string sql = $@"INSERT INTO ReturnRequests_Products (ReturnRequestId, Quantity, UnitPrice, Discount, ProductId)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ReturnRequestId", returnRequestId);
                foreach (var p in parameters) cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los product items", ex);
            }
        }

        private async Task DeleteProductItems(int returnRequestId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"DELETE FROM ReturnRequests_Products WHERE ReturnRequestId = @ReturnRequestId";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ReturnRequestId", returnRequestId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar los product items", ex);
            }
        }


        public async Task<ReturnRequest?> ChangeStatusReturnRequestAsync(ReturnRequest returnRequest)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE ReturnRequests SET 
              Status = @Status,
              ConfirmedDate = @ConfirmedDate
          WHERE Id = @Id",
                returnRequest,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", returnRequest.Id);
                    cmd.Parameters.AddWithValue("@Status", returnRequest.Status.ToString());

                    var p = cmd.Parameters.Add("@ConfirmedDate", System.Data.SqlDbType.DateTime2);
                    p.Value = (object?)returnRequest.ConfirmedDate ?? DBNull.Value;
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la devolución con Id {returnRequest.Id}");

            return returnRequest;
        }
        #endregion
    }
}
