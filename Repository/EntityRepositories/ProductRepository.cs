using BusinessLogic.Común;
using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;

namespace Repository.EntityRepositories
{
    public class ProductRepository : Repository<Product, Product.UpdatableData>, IProductRepository
    {
        public ProductRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger)
        {
        }

        public Task<Product> AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
