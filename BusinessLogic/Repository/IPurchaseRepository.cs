using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        // Métodos adicionales si los necesitas
        Task<Purchase> ChangeStatusPurchase(Purchase purchase);
    }
}