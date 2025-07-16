using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class SubCategoryRepository : Repository<SubCategory, SubCategory.UpdatableData>, ISubCategoryRepository
    {
        public SubCategoryRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<SubCategory> AddAsync(SubCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory> DeleteAsync(SubCategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubCategory>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory> UpdateAsync(SubCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
