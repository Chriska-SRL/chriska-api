
using BusinessLogic.Common;
using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<Delivery> ChangeStatusDeliveryAsync(Delivery delivery);
        Task<List<Delivery>> GetDeliveriesByDistributionIdAsync(int id);
        Task<List<Delivery>> GetDeliveriesPendingByClientIdsAsync(List<int> clientIds);
        Task<List<Delivery>> GetDeliveriesPendingByZoneIdsAsync(List<int> zoneIds);
        Task<List<Delivery>> GetConfirmedByClientIdAsync(int clientId, QueryOptions? options);
    }
}
