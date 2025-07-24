using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
    }
}
