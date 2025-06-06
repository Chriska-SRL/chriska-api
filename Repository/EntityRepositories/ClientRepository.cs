using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Client> logger) : base(connectionString, logger)
        {
        }

        public Client Add(Client entity)
        {
            throw new NotImplementedException();
        }

        public Client? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public Client? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Client Update(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
