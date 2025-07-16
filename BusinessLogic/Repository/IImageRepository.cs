using BusinessLogic.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IImageRepository
    {
        Task<Image> AddAsync(Image entity);
        Task<Image?> DeleteAsync(int id);
        Task<Image?> GetByIdAsync(int id);
        Task<Image?> GetByEntityTypeAndIdAsync(string entityType, int entityId);
        Task<List<Image>> GetAllAsync();
    }
}
