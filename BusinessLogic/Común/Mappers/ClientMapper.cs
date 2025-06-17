using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsZone;
namespace BusinessLogic.Común.Mappers
{
    public static class ClientMapper
    {
        public static Client ToDomain(AddClientRequest addClientRequest)
        {
            return new Client(
                id: 0,
                name: addClientRequest.Name,
                rut: addClientRequest.RUT,
                razonSocial: addClientRequest.RazonSocial,
                address: addClientRequest.Address,
                mapsAddress: addClientRequest.MapsAddress,
                schedule: addClientRequest.Schedule,
                phone: addClientRequest.Phone,
                contactName: addClientRequest.ContactName,
                email: addClientRequest.Email,
                observation: addClientRequest.Observation,
                bankAccount: addClientRequest.BankAccount,
                loanedCrates: addClientRequest.LoanedCrates,
                zone: null, 
                requests: new List<Request>(),
                receipts: new List<Receipt>()
            );

        }
        public static Client.UpdatableData ToUpdatableData(UpdateClientRequest updateClientRequest)
        {
            return new Client.UpdatableData
            {
                Name = updateClientRequest.Name,
                RUT = updateClientRequest.RUT,
                RazonSocial = updateClientRequest.RazonSocial,
                Address = updateClientRequest.Address,
                MapsAddress = updateClientRequest.MapsAddress,
                Schedule = updateClientRequest.Schedule,
                Phone = updateClientRequest.Phone,
                ContactName = updateClientRequest.ContactName,
                Email = updateClientRequest.Email,
                Observation = updateClientRequest.Observations,
                BankAccount = updateClientRequest.BankAccount,
                LoanedCrates = updateClientRequest.LoanedCrates,
                Zone = new Zone
                (
                    id: updateClientRequest.ZoneId,
                    name: "",
                    description: "",
                    days: new List<Day>()
                )

            };
        }

        public static ClientResponse ToResponse(Client client)
        {

            return new ClientResponse
            {
                Name = client.Name,
                RUT = client.RUT,
                RazonSocial = client.RazonSocial,
                Address = client.Address,
                MapsAddress = client.MapsAddress,
                Schedule = client.Schedule,
                Phone = client.Phone,
                ContactName = client.ContactName,
                Email = client.Email,
                Observation = client.Observation,
                BankAccount = client.BankAccount,
                LoanedCrates = client.LoanedCrates,
                zone = new ZoneResponse
                {
                    Id = client.Zone.Id,
                    Name = client.Zone.Name,
                    Description = client.Zone.Description
                }
        };
        }
       
    }
}
