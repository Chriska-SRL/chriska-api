using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class ReceiptRepository : Repository<Receipt, Receipt.UpdatableData>, IReceiptRepository
    {
        public ReceiptRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<Receipt> AddAsync(Receipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<Receipt> DeleteAsync(Receipt entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Receipt>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Receipt?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Receipt> UpdateAsync(Receipt entity)
        {
            throw new NotImplementedException();
        }
    }
}
