using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByName(string name);
    }
}
