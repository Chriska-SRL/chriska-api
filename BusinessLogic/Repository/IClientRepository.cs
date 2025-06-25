using BusinessLogic.Dominio;

namespace BusinessLogic.Repository
{
    public interface IClientRepository:IRepository<Client>
    {
        Client GetByName(string name);
        Client GetByRUT(string rut);
    }
}
