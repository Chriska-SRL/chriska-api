using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Supplier? GetByName(string name);
        Supplier? GetByRUT(string rut);

    }
}
