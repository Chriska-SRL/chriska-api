using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class ShelveRepository : Repository<Shelve, Shelve.UpdatableData>, IShelveRepository
    {
        public ShelveRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Shelve> AddAsync(Shelve entity)
        {
            throw new NotImplementedException();
        }

        public Task<Shelve> DeleteAsync(Shelve entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Shelve>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Shelve?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Shelve> UpdateAsync(Shelve entity)
        {
            throw new NotImplementedException();
        }
    }
}
