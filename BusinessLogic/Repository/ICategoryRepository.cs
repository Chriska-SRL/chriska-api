using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
    }
}
