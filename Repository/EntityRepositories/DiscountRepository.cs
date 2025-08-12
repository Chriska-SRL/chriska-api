using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;
using Repository.Utils;

namespace Repository.EntityRepositories
{
    public class DiscountRepository : Repository<Discount, Discount.UpdatableData>, IDiscountRepository
    {
        public DiscountRepository(string connectionString, AuditLogger auditLogger) : base(connectionString, auditLogger)
        {
        }

        public async Task<Discount> AddAsync(Discount discount)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO Discounts (
                    Description, Percentage, ExpirationDate, Status, ProductQuantity, BrandId, SubCategoryId, ZoneId
                ) OUTPUT INSERTED.Id VALUES (
                    @Description, @Percentage, @ExpirationDate, @Status, @ProductQuantity, @BrandId, @SubCategoryId, @ZoneId
                )",
                discount,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Description", discount.Description);
                    cmd.Parameters.AddWithValue("@Percentage", discount.Percentage);
                    cmd.Parameters.AddWithValue("@ExpirationDate", discount.ExpirationDate);
                    cmd.Parameters.AddWithValue("@Status", discount.Status.ToString());
                    cmd.Parameters.AddWithValue("@ProductQuantity", discount.ProductQuantity);

                    cmd.Parameters.AddWithValue("@BrandId", (object?)discount.Brand?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubCategoryId", (object?)discount.SubCategory?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ZoneId", (object?)discount.Zone?.Id ?? DBNull.Value);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            discount.Id = newId;

            await AddDiscountClientsAsync(discount.Id, discount.Clients);
            await AddDiscountProductsAsync(discount.Id, discount.Products);

            return discount;
        }


        public async Task<Discount> DeleteAsync(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException(nameof(discount), "El descuento no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Discounts SET IsDeleted = 1 WHERE Id = @Id",
                discount,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", discount.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el descuento con Id {discount.Id}");

            return discount;
        }

        string baseQuery = @"
                           SELECT
                                d.*,

                                -- Descuento (prefijo D*)
                                db.Id   AS DBrandId,       db.Name AS DBrandName,       db.Description AS DBrandDescription,
                                dsc.Id  AS DSubCategoryId, dsc.Name AS DSubCategoryName, dsc.Description AS DSubCategoryDescription,
                                dcat.Id AS DCategoryId,    dcat.Name AS DCategoryName,  dcat.Description AS DCategoryDescription,
                                dz.Id   AS DZoneId,        dz.Name AS DZoneName,        dz.Description AS DZoneDescription,
                                dz.ImageUrl AS DZoneImageUrl, dz.DeliveryDays AS DZoneDeliveryDays, dz.RequestDays AS DZoneRequestDays,

                                -- Cliente (prefijo Client*) + Zona del cliente (prefijo CZone*)
                                cl.Id  AS ClientId, cl.Name AS ClientName, cl.RUT AS ClientRUT, cl.RazonSocial AS ClientRazonSocial,
                                cl.Address AS ClientAddress, cl.MapsAddress AS ClientMapsAddress, cl.Schedule AS ClientSchedule,
                                cl.Phone AS ClientPhone, cl.ContactName AS ClientContactName, cl.Email AS ClientEmail,
                                cl.Observations AS ClientObservations, cl.LoanedCrates AS ClientLoanedCrates, cl.Qualification AS ClientQualification,
                                cz.Id AS CZoneId, cz.Name AS CZoneName, cz.Description AS CZoneDescription,
                                cz.ImageUrl AS CZoneImageUrl, cz.DeliveryDays AS CZoneDeliveryDays, cz.RequestDays AS CZoneRequestDays,

                                -- Producto (prefijo Product*) + relaciones del producto (prefijo P*)
                                p.Id AS ProductId, p.Barcode AS ProductBarcode, p.Name AS ProductName,
                                p.Price AS ProductPrice, p.ImageUrl AS ProductImageUrl, p.Stock AS ProductStock,
                                p.AvailableStock AS ProductAvailableStock, p.Description AS ProductDescription,
                                p.UnitType AS ProductUnitType, p.TemperatureCondition AS ProductTemperatureCondition,
                                p.EstimatedWeight AS ProductEstimatedWeight, p.Observations AS ProductObservations,

                                pb.Id   AS PBrandId,       pb.Name AS PBrandName,       pb.Description AS PBrandDescription,
                                psc.Id  AS PSubCategoryId, psc.Name AS PSubCategoryName, psc.Description AS PSubCategoryDescription,
                                pcat.Id AS PCategoryId,    pcat.Name AS PCategoryName,   pcat.Description AS PCategoryDescription,
                                psh.Id  AS PShelveId,      psh.Name AS PShelveName,      psh.Description AS PShelveDescription,
                                pw.Id   AS PWarehouseId,   pw.Name AS PWarehouseName,    pw.Description AS PWarehouseDescription
                            FROM Discounts d
                            LEFT JOIN Brands        db   ON d.BrandId       = db.Id
                            LEFT JOIN SubCategories dsc  ON d.SubCategoryId = dsc.Id
                            LEFT JOIN Categories    dcat ON dsc.CategoryId  = dcat.Id
                            LEFT JOIN Zones         dz   ON d.ZoneId        = dz.Id

                            LEFT JOIN DiscountClients  dcl ON d.Id = dcl.DiscountId
                            LEFT JOIN Clients          cl  ON dcl.ClientId = cl.Id
                            LEFT JOIN Zones            cz  ON cl.ZoneId    = cz.Id

                            LEFT JOIN DiscountProducts dp  ON d.Id = dp.DiscountId
                            LEFT JOIN Products         p   ON dp.ProductId = p.Id
                            LEFT JOIN Brands           pb  ON p.BrandId    = pb.Id
                            LEFT JOIN SubCategories    psc ON p.SubCategoryId = psc.Id
                            LEFT JOIN Categories       pcat ON psc.CategoryId = pcat.Id
                            LEFT JOIN Shelves          psh ON p.ShelveId   = psh.Id
                            LEFT JOIN Warehouses       pw  ON psh.WarehouseId = pw.Id
                            ";

        public async Task<List<Discount>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[]
            {
                "Description", "Status", "BrandId", "SubCategoryId", "ZoneId", "ExpirationDate"
            };

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    var discounts = new Dictionary<int, Discount>();

                    while (reader.Read())
                    {
                        var discountId = (int)reader["Id"];

                        if (!discounts.TryGetValue(discountId, out var discount))
                        {
                            discount = DiscountMapper.FromReader(reader); 
                            discounts[discountId] = discount;
                        }

                        AccumulateRelationsForDiscount(reader, discount);
                    }

                    return discounts.Values.ToList();
                },
                options: options,
                tableAlias: "d",
                allowedFilterColumns: allowedFilters
            );
        }


        public async Task<Discount?> GetByIdAsync(int id)
        {
            return await GetByFieldAsync("Id", id);
        }

        private async Task<Discount?> GetByFieldAsync(string fieldName, object value)
        {
            return await ExecuteReadAsync(
                baseQuery: $"{baseQuery} WHERE d.{fieldName} = @Value",
                map: reader =>
                {
                    Discount? discount = null;
                    while (reader.Read())
                    {
                        discount ??= DiscountMapper.FromReader(reader);
                        AccumulateRelationsForDiscount(reader, discount);
                    }
                    return discount;
                },
                options: new QueryOptions(),
                tableAlias: "d",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Value", value)
            );
        }


        public async Task<Discount> UpdateAsync(Discount discount)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Discounts SET 
                    Description = @Description, Percentage = @Percentage, ExpirationDate = @ExpirationDate, Status = @Status,
                    ProductQuantity = @ProductQuantity, 
                    BrandId = @BrandId, SubCategoryId = @SubCategoryId, ZoneId = @ZoneId
                WHERE Id = @Id",
                discount,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", discount.Id);
                    cmd.Parameters.AddWithValue("@Description", discount.Description);
                    cmd.Parameters.AddWithValue("@Percentage", discount.Percentage);
                    cmd.Parameters.AddWithValue("@ExpirationDate", discount.ExpirationDate);
                    cmd.Parameters.AddWithValue("@Status", discount.Status.ToString());
                    cmd.Parameters.AddWithValue("@ProductQuantity", discount.ProductQuantity);

                    cmd.Parameters.AddWithValue("@BrandId", (object?)discount.Brand?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubCategoryId", (object?)discount.SubCategory?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ZoneId", (object?)discount.Zone?.Id ?? DBNull.Value);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el descuento con Id {discount.Id}");

            await DeleteDiscountClientsAsync(discount.Id);
            await AddDiscountClientsAsync(discount.Id, discount.Clients);

            await DeleteDiscountProductsAsync(discount.Id);
            await AddDiscountProductsAsync(discount.Id, discount.Products);

            return discount;
        }
        private async Task DeleteDiscountClientsAsync(int discountId)
        {
            await ExecuteWriteAsync(
                "DELETE FROM DiscountClients WHERE DiscountId = @DiscountId",
                cmd => cmd.Parameters.AddWithValue("@DiscountId", discountId)
            );
        }
        private async Task AddDiscountClientsAsync(int discountId, List<Client> clients)
        {
            if (clients?.Any() != true) return;

            await ExecuteWriteAsync(
                QueryBuilder.BuildBulkInsertQuery("DiscountClients", "DiscountId", "ClientId", clients.Count),
                cmd =>
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@DiscountId{i}", discountId);
                        cmd.Parameters.AddWithValue($"@ClientId{i}", clients[i].Id);
                    }
                }
            );
        }
        private async Task DeleteDiscountProductsAsync(int discountId)
        {
            await ExecuteWriteAsync(
                "DELETE FROM DiscountProducts WHERE DiscountId = @DiscountId",
                cmd => cmd.Parameters.AddWithValue("@DiscountId", discountId)
            );
        }
        private async Task AddDiscountProductsAsync(int discountId, List<Product> products)
        {
            if (products?.Any() != true) return;

            await ExecuteWriteAsync(
                QueryBuilder.BuildBulkInsertQuery("DiscountProducts", "DiscountId", "ProductId", products.Count),
                cmd =>
                {
                    for (int i = 0; i < products.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@DiscountId{i}", discountId);
                        cmd.Parameters.AddWithValue($"@ProductId{i}", products[i].Id);
                    }
                }
            );
        }

        private void AccumulateRelationsForDiscount(SqlDataReader reader, Discount discount)
        {
            discount.Clients ??= new List<Client>();
            discount.Products ??= new List<Product>();

            if (reader["ClientId"] != DBNull.Value)
            {
                var clientId = (int)reader["ClientId"];
                if (!discount.Clients.Any(x => x.Id == clientId))
                {
                    var client = ClientMapper.FromReader(reader, "Client");
                    if (client != null) discount.Clients.Add(client);
                }
            }

            if (reader["ProductId"] != DBNull.Value)
            {
                var productId = (int)reader["ProductId"];
                if (!discount.Products.Any(x => x.Id == productId))
                {
                    var product = ProductMapper.FromReader(reader, "Product", "P");
                    if (product != null) discount.Products.Add(product);
                }
            }
        }

    }
}
