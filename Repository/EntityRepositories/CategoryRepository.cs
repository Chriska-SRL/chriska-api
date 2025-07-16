using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class CategoryRepository : Repository<Category, Category.UpdatableData>, ICategoryRepository
    {
        public CategoryRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Category> AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<Category> DeleteAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }
    
    }
}
