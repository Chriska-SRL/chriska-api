using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class StockMovementRepository : Repository<StockMovement, StockMovement.UpdatableData>, IStockMovementRepository
    {
        public StockMovementRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public Task<StockMovement> AddAsync(StockMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<StockMovement> DeleteAsync(StockMovement entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockMovement>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<StockMovement?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<StockMovement> UpdateAsync(StockMovement entity)
        {
            throw new NotImplementedException();
        }
    }
}
