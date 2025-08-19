using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IDiscountRepository : IRepository<Domain.Discount>
    {
        Task<Discount?> GetBestByProductAndClientAsync(Product product, Client client);
    }
}
