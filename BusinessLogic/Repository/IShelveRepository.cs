using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IShelveRepository : IRepository<Shelve>
    {
        Task<Shelve> GetByNameAsync(string name);
    }
}
