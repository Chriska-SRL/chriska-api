using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Common;

namespace Repository.EntityRepositories
{
    public class VehicleCostRepository : Repository<VehicleCost, VehicleCost.UpdatableData>, IVehicleCostRepository
    {
        public VehicleCostRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<VehicleCost> AddAsync(VehicleCost entity)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleCost> DeleteAsync(VehicleCost entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehicleCost>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleCost?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<VehicleCost> UpdateAsync(VehicleCost entity)
        {
            throw new NotImplementedException();
        }
    }
}
