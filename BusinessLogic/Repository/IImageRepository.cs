using BusinessLogic.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IImageRepository
    {
        Task<Image> AddAsync(Image image);
        Task<Image> UpdateAsync(Image image);
        Task<bool> DeleteAsync(int imageId);
        Task<Image?> GetByEntityTypeAndIdAsync(string entityType, int entityId);
    }
}
