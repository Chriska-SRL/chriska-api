using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class OrderRepository : Repository<Order, Order.UpdatableData>, IOrderRepository
    {
        public OrderRepository(string connectionString, AuditLogger auditLogger) : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Order> AddAsync(Order order)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Orders (Id, Date, Observations, Status, ClientId, Crates, OrderRequestId) " +
                "OUTPUT INSERTED.Id VALUES (@Id, @Date, @Observations, @Status, @ClientId, @Crates, @OrderRequestId)",
                order,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", order.Id);
                    cmd.Parameters.AddWithValue("@Date", order.Date);
                    cmd.Parameters.AddWithValue("@Observations", (object?)order.Observations ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status.ToString());
                    cmd.Parameters.AddWithValue("@ClientId", order.Client.Id);
                    cmd.Parameters.AddWithValue("@Crates", order.Crates);
                    cmd.Parameters.AddWithValue("@OrderRequestId", (object?)order.OrderRequest?.Id ?? DBNull.Value);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            await AddProductItems(newId, order.ProductItems);

            order.Id = newId;
            return order;
        }


        #endregion

        #region Update

        public async Task<Order> UpdateAsync(Order order)
        {
            await DeleteProductItems(order.Id);
            await AddProductItems(order.Id, order.ProductItems);

            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Orders SET 
                        Observations = @Observations,
                        Crates = @Crates
                  WHERE Id = @Id",
                order,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", order.Id);
                    cmd.Parameters.AddWithValue("@Observations", (object?)order.Observations ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Crates", order.Crates);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la orden con Id {order.Id}");

            return order;
        }

        #endregion

        #region Delete

        public async Task<Order> DeleteAsync(Order entity)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Orders SET IsDeleted = 1 WHERE Id = @Id",
                entity,
                AuditAction.Delete,
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", entity.Id)
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la orden con Id {entity.Id}");

            return entity;
        }

        #endregion

        #region Query (base)

        private readonly string baseQuery = @"
            SELECT
                -- Order
                o.*,

                -- Client (prefijo Client)
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

                -- Zone (prefijo Zone)
                z.Name            AS ZoneName,
                z.Description     AS ZoneDescription,
                z.DeliveryDays    AS ZoneDeliveryDays,
                z.RequestDays     AS ZoneRequestDays,
                z.ImageUrl        AS ZoneImageUrl,

                -- User (prefijo User)
                u.Id              AS UserId,
                u.Name            AS UserName,
                u.Username        AS UserUsername,
                u.Password        AS UserPassword,
                u.IsEnabled       AS UserIsEnabled,
                u.NeedsPasswordChange AS UserNeedsPasswordChange,

                -- Role (prefijo UserRole)
                r.Id              AS UserRoleId,
                r.Name            AS UserRoleName,
                r.Description     AS UserRoleDescription,

                -- Items (prefijo OP_)
                op.OrderId        AS OP_OrderId,
                op.Quantity       AS OP_Quantity,
                op.UnitPrice      AS OP_UnitPrice,
                op.Discount       AS OP_Discount,
                op.Weight         AS OP_Weight,

                -- Product (prefijo Product)
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
                cat.Description    AS CategoryDescription,

                -- OrderRequest (prefijo ORq_)
                orq.Id             AS OrderRequestId,
                orq.Date           AS OrderRequestDate,
                orq.Observations   AS OrderRequestObservations,
                orq.Status         AS OrderRequestStatus,
                orq.ClientId       AS OrderRequestClientId,
                orq.ConfirmedDate AS OrderRequestConfirmedDate


            FROM Orders o
            LEFT JOIN Clients c       ON c.Id = o.ClientId
            LEFT JOIN Zones z         ON z.Id = c.ZoneId
            LEFT JOIN Users u         ON u.Id = o.CreatedBy
            LEFT JOIN Roles r         ON r.Id = u.RoleId
            LEFT JOIN Orders_Products op ON op.OrderId = o.Id
            LEFT JOIN Products p      ON p.Id = op.ProductId
            LEFT JOIN Brands b        ON b.Id = p.BrandId
            LEFT JOIN SubCategories sb ON sb.Id = p.SubCategoryId
            LEFT JOIN Categories cat   ON cat.Id = sb.CategoryId
            LEFT JOIN OrderRequests orq ON orq.Id = o.OrderRequestId
        ";

        #endregion

        #region GetAll
        public async Task<List<Order>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "Observations", "Status", "ConfirmedDate", "ClientId", "OrderRequestId", "Crates" };
            var dict = new Dictionary<int, Order>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(id, out var order))
                        {
                            order = OrderMapper.FromReader(reader);
                            dict.Add(id, order);
                        }

                        var item = ProductItemMapper.FromReader(reader, "OP_");
                        if (item is not null) order!.ProductItems.Add(item);
                    }
                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "o",
                allowedFilterColumns: allowedFilters
            );
        }
        #endregion

        #region GetById

        public async Task<Order?> GetByIdAsync(int id)
        {
            var dict = new Dictionary<int, Order>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE o.Id = @Id",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int ordId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(ordId, out var order))
                        {
                            order = OrderMapper.FromReader(reader);
                            dict.Add(ordId, order);
                        }

                        var item = ProductItemMapper.FromReader(reader, "OP_");
                        if (item is not null) order!.ProductItems.Add(item);
                    }

                    return dict.Values.FirstOrDefault();
                },
                options: new QueryOptions(),
                tableAlias: "o",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion

        #region Private helpers (items)

        private async Task AddProductItems(int orderId, List<ProductItem> productItems)
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
                    values.Add($"(@OrderId, @Quantity{i}, @UnitPrice{i}, @Discount{i}, @ProductId{i}, @Weight{i})");

                    parameters.Add(new SqlParameter($"@Quantity{i}", productItem.Quantity));
                    parameters.Add(new SqlParameter($"@UnitPrice{i}", productItem.UnitPrice));
                    parameters.Add(new SqlParameter($"@Discount{i}", productItem.Discount));
                    parameters.Add(new SqlParameter($"@ProductId{i}", productItem.Product.Id));
                    parameters.Add(new SqlParameter($"@Weight{i}",(object?)productItem.Weight ?? DBNull.Value));
                    i++;
                }

                string sql = $@"INSERT INTO Orders_Products (OrderId, Quantity, UnitPrice, Discount, ProductId, Weight)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                foreach (var p in parameters) cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los product items de la orden", ex);
            }
        }

        private async Task DeleteProductItems(int orderId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"DELETE FROM Orders_Products WHERE OrderId = @OrderId";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar los product items de la orden", ex);
            }
        }

        public async Task<Order?> ChangeStatusOrder(Order order)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Orders SET 
                    DeliveryId = @DeliveryId,
                    Status = @Status,
                    ConfirmedDate = @ConfirmedDate
                WHERE Id = @Id",
                order,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", order.Id);
                    cmd.Parameters.AddWithValue("@DeliveryId", (object?)order.Delivery?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", order.Status.ToString());
                    var p = cmd.Parameters.Add("@ConfirmedDate", System.Data.SqlDbType.DateTime2);
                    p.Value = (object?)order.ConfirmedDate ?? DBNull.Value;
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la orden con Id {order.Id}");

            return order;
        }

        #endregion
    }
}

