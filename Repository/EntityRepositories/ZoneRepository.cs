using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Zone> logger) : base(connectionString, logger)
        {
        }

        public Zone Add(Zone entity)
        {
            throw new NotImplementedException();
        }

        public Zone? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Zone> GetAll()
        {
            throw new NotImplementedException();
        }

        public Zone? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Zone Update(Zone entity)
        {
            throw new NotImplementedException();
        }
    }
}
