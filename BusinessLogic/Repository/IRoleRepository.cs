using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByIdWithUsersAsync(int id);
        Task<Role?> GetByNameAsync(string name);
    }
}
