using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {

        private List<Client> Clients = new List<Client>();
        private List<Receipt> Receipts = new List<Receipt>();

        private readonly IClientRepository _clientRepository;
        public ClientsSubSystem(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
    }

}
