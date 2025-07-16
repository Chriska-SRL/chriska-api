using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class UserRepository : Repository<User, User.UpdatableData>, IUserRepository
    {
        public UserRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public bool ExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public User? GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
