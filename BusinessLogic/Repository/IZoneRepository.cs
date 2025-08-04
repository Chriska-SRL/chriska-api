using BusinessLogic.Domain;

namespace BusinessLogic.Repository
{
    public interface IZoneRepository: IRepository<Zone>
    {
        Task<string> UpdateImageUrlAsync(Zone zone, string imageUrl);
    }
}
