using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IOrderRequestRepository : IRepository<OrderRequest>
    {
        Task<OrderRequest?> ChangeStatusOrderRequest(OrderRequest orderRequest);
    }
}
