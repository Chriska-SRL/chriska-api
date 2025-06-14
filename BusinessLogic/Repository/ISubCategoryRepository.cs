using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        SubCategory GetByName(string name);
    }
}
