using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(string connectionString, ILogger<Sale> logger) : base(connectionString, logger)
        {
        }

        public Sale Add(Sale entity)
        {
            throw new NotImplementedException();
        }

        public Sale? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Sale> GetAll()
        {
            throw new NotImplementedException();
        }

        public Sale? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Sale Update(Sale entity)
        {
            throw new NotImplementedException();
        }
    }
}
