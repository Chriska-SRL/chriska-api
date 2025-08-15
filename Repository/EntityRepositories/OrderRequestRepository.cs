using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class OrderRequestRepository : Repository<OrderRequest, OrderRequest.UpdatableData>, IOrderRequestRepository
    {
        public OrderRequestRepository(string connectionString, AuditLogger auditLogger) : base(connectionString, auditLogger)
        {
        }

        public async Task<OrderRequest> AddAsync(OrderRequest orderRequest)
        {
            int newId = await ExecuteWriteWithAuditAsync(
            "INSERT INTO OrderRequests ( Date,  Observations,  Status,  ClientId)" +
            "OUTPUT INSERTED.Id Values (@Date, @Observations, @Status, @ClientId)",
            orderRequest,
            AuditAction.Insert,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Date", orderRequest.Date);
                cmd.Parameters.AddWithValue("@Observations", orderRequest.Observations);
                cmd.Parameters.AddWithValue("@Status", orderRequest.Status.ToString());
                cmd.Parameters.AddWithValue("@ClientId", orderRequest.Client.Id);

            },
            async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
        );

            await AddProductItems(newId, orderRequest.ProductItems);

            orderRequest.Id = newId;
            return orderRequest;
        }

        private async Task AddProductItems(int orderRequestId, List<ProductItem> productItems)
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
                    values.Add($"(@OrderRequestId, @Quantity{i}, @UnitPrice{i}, @Discount{i}, @ProductId{i})");

                    parameters.Add(new SqlParameter($"@Quantity{i}", productItem.Quantity));
                    parameters.Add(new SqlParameter($"@UnitPrice{i}", productItem.UnitPrice));
                    parameters.Add(new SqlParameter($"@Discount{i}", productItem.Discount));
                    parameters.Add(new SqlParameter($"@ProductId{i}", productItem.Product.Id));
                    i++;
                }

                string sql = $@"INSERT INTO OrderRequests_Products (OrderRequestId, Quantity, UnitPrice, Discount, ProductId)
                                VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderRequestId", orderRequestId);
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los product items", ex);
            }
        }

        public Task<OrderRequest> DeleteAsync(OrderRequest entity)
        {
            throw new NotImplementedException();
        }

        string baseQuery = @"SELECT
                                -- OrderRequest
                                orq.*,

                                -- Client (prefijo Client)
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

                                -- Items (prefijo ORP_)
                                orp.OrderRequestId AS ORP_OrderRequestId,
                                orp.Quantity       AS ORP_Quantity,
                                orp.UnitPrice      AS ORP_UnitPrice,
                                orp.Discount       AS ORP_Discount,
                                orp.Weight         AS ORP_Weight,

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
                                cat.Description    AS CategoryDescription

                            FROM OrderRequests orq
                            LEFT JOIN Clients c       ON c.Id = orq.ClientId
                            LEFT JOIN Zones z         ON z.Id = c.ZoneId
                            LEFT JOIN Users u         ON u.Id = orq.CreatedBy
                            LEFT JOIN Roles r         ON r.Id = u.RoleId
                            LEFT JOIN OrderRequests_Products orp ON orp.OrderRequestId = orq.Id
                            LEFT JOIN Products p      ON p.Id = orp.ProductId
                            LEFT JOIN Brands b        ON b.Id = p.BrandId
                            LEFT JOIN SubCategories sb ON sb.Id = p.SubCategoryId
                            LEFT JOIN Categories cat   ON cat.Id = sb.CategoryId
                            ";

        public async Task<List<OrderRequest>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "Observations", "Status", "ConfirmedDate", "ClientId", "OrderId" };
            var dict = new Dictionary<int, OrderRequest>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(id, out var orq))
                        {
                            orq = OrderRequestMapper.FromReader(reader);
                            dict.Add(id, orq);
                        }

                        var item = ProductItemMapper.FromReader(reader, "ORP_");
                        if (item is not null) orq!.ProductItems.Add(item);
                    }
                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "orq",
                allowedFilterColumns: allowedFilters
            );
        }



        public Task<OrderRequest?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderRequest> UpdateAsync(OrderRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
