using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone, Zone.UpdatableData>, IZoneRepository
    {
        public ZoneRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Zone> AddAsync(Zone entity)
        {
            throw new NotImplementedException();
        }

        public Task<Zone> DeleteAsync(Zone entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Zone>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Zone?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Zone> UpdateAsync(Zone entity)
        {
            throw new NotImplementedException();
        }
    }
}
