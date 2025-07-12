using BusinessLogic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IImageRepository
    {
        Image Add(Image entity);
        Image? Delete(int id);
        Image? GetById(int id);
        Image? GetByEntityTypeAndId(string entityType, int entityId);
        List<Image> GetAll();
    }
}
