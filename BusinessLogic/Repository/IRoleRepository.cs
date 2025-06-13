using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetByName(string name);
    }
}
