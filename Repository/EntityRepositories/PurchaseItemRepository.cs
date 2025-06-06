using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class PurchaseItemRepository : Repository<PurchaseItem>, IPurchaseItemRepository
    {
        public PurchaseItemRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<PurchaseItem> logger) : base(connectionString, logger)
        {
        }

        public PurchaseItem Add(PurchaseItem entity)
        {
            throw new NotImplementedException();
        }

        public PurchaseItem? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<PurchaseItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public PurchaseItem? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public PurchaseItem Update(PurchaseItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
