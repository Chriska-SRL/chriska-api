using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetByNameAsync(string name);
        Task<Client> GetByRUTAsync(string rUT);
    }
}
