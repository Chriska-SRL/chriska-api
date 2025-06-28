using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IStockMovementRepository : IRepository<StockMovement>
    {
        List<StockMovement> GetAll(DateTime from, DateTime to);
        List<StockMovement> GetAllByShelve(int id, DateTime from, DateTime to);
        List<StockMovement> GetAllByWarehouse(int id, DateTime from, DateTime to);
    }
}
