using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;

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
                "INSERT INTO Discounts (Description, Percentage, ExpirationDate, Status, ProductQuantity) OUTPUT INSERTED.Id VALUES (@Description, @Percentage, @ExpirationDate, @Status, @ProductQuantity)",
                discount,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Description", discount.Description);
                    cmd.Parameters.AddWithValue("@Percentage", discount.Percentage);
                    cmd.Parameters.AddWithValue("@ExpirationDate", discount.ExpirationDate);
                    cmd.Parameters.AddWithValue("@Status", discount.Status);
                    cmd.Parameters.AddWithValue("@ProductQuantity", discount.ProductQuantity);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            foreach (var product in discount.Products)
            {
                await ExecuteWriteAsync(
                    "INSERT INTO DiscountProducts (DiscountId, ProductId) VALUES (@DiscountId, @ProductId)",
                    configureCommand: cmd =>
                    {
                        cmd.Parameters.AddWithValue("@DiscountId", newId);
                        cmd.Parameters.AddWithValue("@ProductId", product.Id);
                    }
                );
            }

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            discount.Id = newId;
            return discount;
        }

        public Task<Discount> DeleteAsync(Discount entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Discount>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Discount?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Discount> UpdateAsync(Discount entity)
        {
            throw new NotImplementedException();
        }
    }
}
