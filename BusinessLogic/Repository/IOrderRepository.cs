using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IOrderRepository:IRepository<Order>
    {
        public Task<Order> ChangeStatusOrder(Order order);
    }
}
