using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
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

            -- Order
            o.Id              AS OrderId,
            o.Date            AS OrderDate,
            o.ConfirmedDate   AS OrderConfirmedDate,
            o.Status          AS OrderStatus,
            o.Observations    AS OrderObservations,
            o.Crates        AS OrderCrates,

            -- Items (prefijo DP_)
                dp.DeliveryId        AS DP_DeliveryId,
                dp.Quantity       AS DP_Quantity,
                dp.UnitPrice      AS DP_UnitPrice,
                dp.Discount       AS DP_Discount,
                dp.Weight         AS DP_Weight,

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

        FROM Deliveries d
        LEFT JOIN Clients c  ON c.Id = d.ClientId
        LEFT JOIN Zones z    ON z.Id = c.ZoneId
        LEFT JOIN Users u    ON u.Id = d.CreatedBy
        LEFT JOIN Roles r    ON r.Id = u.RoleId
        LEFT JOIN Orders o   ON o.Id = d.OrderId
        LEFT JOIN Deliveries_Products dp ON dp.DeliveryId = d.Id
        LEFT JOIN Products p ON dp.ProductId = p.Id  
        LEFT JOIN Brands b        ON b.Id = p.BrandId
        LEFT JOIN SubCategories sb ON sb.Id = p.SubCategoryId
        LEFT JOIN Categories cat   ON cat.Id = sb.CategoryId
        

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

            await AddDeliveryItems(newId, delivery.ProductItems);

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

                        var item = ProductItemMapper.FromReader(reader, "DP_");
                        if (item is not null) delivery!.ProductItems.Add(item);
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
            var dict = new Dictionary<int, Delivery>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE d.Id = @Id",
                map: reader =>
                {
                    
                    while (reader.Read())
                    {
                        int delId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(delId, out var delivery))
                        {
                            delivery = DeliveryMapper.FromReader(reader);
                            dict.Add(delId, delivery);
                        }

                        var item = ProductItemMapper.FromReader(reader, "DP_");
                        if (item is not null) delivery!.ProductItems.Add(item);
                    }

                    return dict.Values.FirstOrDefault();
                },
                options: new QueryOptions(),
                tableAlias: "d",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }


        #endregion

        #region Update

        public async Task<Delivery> UpdateAsync(Delivery delivery)
        {

            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Deliveries SET 
                        Observations = @Observations
                  WHERE Id = @Id",
                delivery,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", delivery.Id);
                    cmd.Parameters.AddWithValue("@Observations", delivery.Observations);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el delivery con Id {delivery.Id}");

            return delivery;
        }

        #endregion

        public async Task<Delivery?> ChangeStatusDeliveryAsync(Delivery delivery)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Deliveries SET 
                    Status = @Status,
                    ConfirmedDate = @ConfirmedDate
                WHERE Id = @Id",
                delivery,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", delivery.Id);
                    cmd.Parameters.AddWithValue("@Status", delivery.Status.ToString());
                    var p = cmd.Parameters.Add("@ConfirmedDate", System.Data.SqlDbType.DateTime2);
                    p.Value = (object?)delivery.ConfirmedDate ?? DBNull.Value;
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la solicitud con Id {delivery.Id}");

            return delivery;
        }

        #region Private helpers (items)

        public async Task AddDeliveryItems(int deliveryId, List<ProductItem> productItems)
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

                foreach (var item in productItems)
                {
                    values.Add($"(@DeliveryId, @Quantity{i}, @UnitPrice{i}, @Discount{i}, @ProductId{i}, @Weight{i})");

                    parameters.Add(new SqlParameter($"@Quantity{i}", item.Quantity));
                    parameters.Add(new SqlParameter($"@UnitPrice{i}", item.UnitPrice));
                    parameters.Add(new SqlParameter($"@Discount{i}", item.Discount));
                    parameters.Add(new SqlParameter($"@ProductId{i}", item.Product.Id));
                    parameters.Add(new SqlParameter($"@Weight{i}", (object?)item.Weight ?? DBNull.Value));
                    i++;
                }

                string sql = $@"INSERT INTO Deliveries_Products (DeliveryId, Quantity, UnitPrice, Discount, ProductId, Weight)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@DeliveryId", deliveryId);
                foreach (var p in parameters) cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los delivery items", ex);
            }
        }
        #endregion
    
    }
}
