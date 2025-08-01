using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        Task<SubCategory?> GetByNameAsync(string name);
    }
}
