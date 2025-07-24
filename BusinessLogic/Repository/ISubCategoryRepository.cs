using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        Task<SubCategory?> GetByNameAsync(string name);
    }
}
