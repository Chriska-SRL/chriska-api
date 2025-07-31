using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IShelveRepository : IRepository<Shelve>
    {
        Task<Shelve> GetByNameAsync(string name);
    }
}
