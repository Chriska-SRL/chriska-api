using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Common;

namespace Repository.EntityRepositories
{
    public class VehicleRepository : Repository<Vehicle, Vehicle.UpdatableData>, IVehicleRepository
    {
        public VehicleRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Vehicle> AddAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> DeleteAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vehicle>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> UpdateAsync(Vehicle entity)
        {
            throw new NotImplementedException();
        }
    }
}
