using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class PurchaseRepository : Repository<Purchase, Purchase.UpdatableData>, IPurchaseRepository
    {
        public PurchaseRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Purchase> AddAsync(Purchase entity)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> DeleteAsync(Purchase entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Purchase>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> UpdateAsync(Purchase entity)
        {
            throw new NotImplementedException();
        }
    }
}
