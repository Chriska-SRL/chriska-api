using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class RoleRepository : Repository<Role, Role.UpdatableData>, IRoleRepository
    {
        public RoleRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Role> AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<Role> DeleteAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Role> UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
