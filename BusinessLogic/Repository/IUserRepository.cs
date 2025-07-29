using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User?> GetByUsernameAsync(string username);
    }
}
