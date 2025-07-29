using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsClient;
using Microsoft.VisualBasic.FileIO;
using BusinessLogic.DTOs;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class ClientsSubSystem
    {
        private readonly IClientRepository _clientRepository;
        private readonly IZoneRepository _zoneRepository;

        public ClientsSubSystem(IClientRepository clientRepository, IZoneRepository zoneRepository)
        {
            _clientRepository = clientRepository;
            _zoneRepository = zoneRepository;
        }

        // Clientes

        public async Task<ClientResponse> AddClientAsync(AddClientRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientResponse> UpdateClientAsync(UpdateClientRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientResponse> DeleteClientAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientResponse> GetClientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClientResponse>> GetAllClientsAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

    }
}
