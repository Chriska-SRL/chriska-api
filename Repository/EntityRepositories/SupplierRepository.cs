using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class SupplierRepository : Repository<Supplier, Supplier.UpdatableData>, ISupplierRepository
    {
        public SupplierRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Supplier> AddAsync(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> DeleteAsync(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Supplier>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> UpdateAsync(Supplier entity)
        {
            throw new NotImplementedException();
        }
    }
}
