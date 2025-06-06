using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ShelveRepository : Repository<Shelve>, IShelveRepository
    {
        public ShelveRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Shelve> logger) : base(connectionString, logger)
        {
        }

        public Shelve Add(Shelve entity)
        {
            throw new NotImplementedException();
        }

        public Shelve? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Shelve> GetAll()
        {
            throw new NotImplementedException();
        }

        public Shelve? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Shelve Update(Shelve entity)
        {
            throw new NotImplementedException();
        }
    }
}
