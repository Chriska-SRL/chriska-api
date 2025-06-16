using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role? GetByIdWithUsers(int id);
        Role? GetByName(string name);
    }
}
