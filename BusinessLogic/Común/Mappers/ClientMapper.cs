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
                observations: addClientRequest.Observations,
                bank: addClientRequest.Bank,
                bankAccount: addClientRequest.BankAccount,
                loanedCrates: addClientRequest.LoanedCrates,
                zone: new Zone( addClientRequest.ZoneId)
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
                Observations= updateClientRequest.Observations,
                Bank = updateClientRequest.Bank,
                BankAccount = updateClientRequest.BankAccount,
                LoanedCrates = updateClientRequest.LoanedCrates
            };
        }

        public static ClientResponse ToResponse(Client client)
        {

            return new ClientResponse
            {
                Id=client.Id,
                Name = client.Name,
                RUT = client.RUT,
                RazonSocial = client.RazonSocial,
                Address = client.Address,
                MapsAddress = client.MapsAddress,
                Schedule = client.Schedule,
                Phone = client.Phone,
                ContactName = client.ContactName,
                Email = client.Email,
                Observations = client.Observations,
                Bank = client.Bank,
                BankAccount = client.BankAccount,
                LoanedCrates = client.LoanedCrates,
                Zone = ZoneMapper.ToResponse(client.Zone)
            };
        }
       
    }
}
