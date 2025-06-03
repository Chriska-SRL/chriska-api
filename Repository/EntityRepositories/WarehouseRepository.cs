using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Warehouse> logger) : base(connectionString, logger)
        {
        }

        public Warehouse Add(Warehouse entity)
        {
            throw new NotImplementedException();
        }

        public Warehouse? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Warehouse> GetAll()
        {
            throw new NotImplementedException();
        }

        public Warehouse? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Warehouse Update(Warehouse entity)
        {
            throw new NotImplementedException();
        }
    }
}
