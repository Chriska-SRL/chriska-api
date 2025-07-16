using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class ClientRepository : Repository<Client, Client.UpdatableData>, IClientRepository
    {
        public ClientRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Client> AddAsync(Client entity)
        {
            throw new NotImplementedException();
        }

        public Task<Client> DeleteAsync(Client entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Client?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Client> UpdateAsync(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
