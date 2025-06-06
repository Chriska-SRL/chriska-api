using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Purchase> logger) : base(connectionString, logger)
        {
        }

        public Purchase Add(Purchase entity)
        {
            throw new NotImplementedException();
        }

        public Purchase? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Purchase> GetAll()
        {
            throw new NotImplementedException();
        }

        public Purchase? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Purchase Update(Purchase entity)
        {
            throw new NotImplementedException();
        }
    }
}
