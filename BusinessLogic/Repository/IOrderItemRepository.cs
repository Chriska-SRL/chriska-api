using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    //Mathias: Esta Interface puede que no sea necesaria, ya que puede que se resuelva con la interfaz IOrderRepository.
    public interface IOrderItemRepository:IRepository<OrderItem>
    {
    }
}
