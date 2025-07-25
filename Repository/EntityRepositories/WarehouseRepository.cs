using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class WarehouseRepository : Repository<Warehouse, Warehouse.UpdatableData>, IWarehouseRepository
    {
        public WarehouseRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Warehouse> AddAsync(Warehouse entity)
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse> DeleteAsync(Warehouse entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Warehouse>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse> UpdateAsync(Warehouse entity)
        {
            throw new NotImplementedException();
        }
    }
}
