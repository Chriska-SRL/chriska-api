using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
