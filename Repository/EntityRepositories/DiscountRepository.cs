using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
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
                    Description, Percentage, ExpirationDate, Status, ProductQuantity,
                    DiscountProductType, DiscountClientType, BrandId, SubCategoryId, ZoneId
                ) OUTPUT INSERTED.Id VALUES (
                    @Description, @Percentage, @ExpirationDate, @Status, @ProductQuantity,
                    @DiscountProductType, @DiscountClientType, @BrandId, @SubCategoryId, @ZoneId
                )",
                discount,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Description", discount.Description);
                    cmd.Parameters.AddWithValue("@Percentage", discount.Percentage);
                    cmd.Parameters.AddWithValue("@ExpirationDate", discount.ExpirationDate);
                    cmd.Parameters.AddWithValue("@Status", discount.Status);
                    cmd.Parameters.AddWithValue("@ProductQuantity", discount.ProductQuantity);
                    cmd.Parameters.AddWithValue("@DiscountProductType", discount.discountProductType.ToString());
                    cmd.Parameters.AddWithValue("@DiscountClientType", discount.discountClientType.ToString());

                    cmd.Parameters.AddWithValue("@BrandId", (object?)discount.Brand?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubCategoryId", (object?)discount.SubCategory?.Id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ZoneId", (object?)discount.Zone?.Id ?? DBNull.Value);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            await AddDiscountClientsAsync(discount.Id, discount.Clients);
            await AddDiscountProductsAsync(discount.Id, discount.Products);

            discount.Id = newId;
            return discount;
        }


        public async Task<Discount> DeleteAsync(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException(nameof(discount), "El producto no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Discount SET IsDeleted = 1 WHERE Id = @Id",
                discount,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", discount.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el producto con Id {discount.Id}");

            return discount;
        }

        public Task<List<Discount>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Discount?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Discount> UpdateAsync(Discount discount)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Discounts SET 
                    Description = @Description, Percentage = @Percentage, ExpirationDate = @ExpirationDate, Status = @Status,
                    ProductQuantity = @ProductQuantity, DiscountProductType = @DiscountProductType, DiscountClientType = @DiscountClientType,
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
                    cmd.Parameters.AddWithValue("@Status", discount.Status);
                    cmd.Parameters.AddWithValue("@ProductQuantity", discount.ProductQuantity);
                    cmd.Parameters.AddWithValue("@DiscountProductType", discount.discountProductType.ToString());
                    cmd.Parameters.AddWithValue("@DiscountClientType", discount.discountClientType.ToString());

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

    }
}
