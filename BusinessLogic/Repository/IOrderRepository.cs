using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> ChangeStatusOrder(Order order);
    }
}
