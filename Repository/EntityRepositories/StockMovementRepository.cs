using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class StockMovementRepository : Repository<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(string connectionString, ILogger<StockMovement> logger) : base(connectionString, logger)
        {
        }

        public StockMovement Add(StockMovement entity)
        {
            throw new NotImplementedException();
        }

        public StockMovement? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<StockMovement> GetAll()
        {
            throw new NotImplementedException();
        }

        public StockMovement? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public StockMovement Update(StockMovement entity)
        {
            throw new NotImplementedException();
        }
    }
}
