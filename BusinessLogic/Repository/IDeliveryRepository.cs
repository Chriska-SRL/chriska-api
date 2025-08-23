
using BusinessLogic.Common;
using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<Delivery> ChangeStatusDeliveryAsync(Delivery delivery);
        Task<List<Delivery>> GetConfirmedByClientIdAsync(int clientId, QueryOptions? options);
    }
}
