using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        bool ExistsByUsername(string username);
        User? GetByUsername(string username);
    }
}
