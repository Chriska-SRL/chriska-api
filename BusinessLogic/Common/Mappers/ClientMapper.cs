using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsClient;

namespace BusinessLogic.Común.Mappers
{
    public static class ClientMapper
    {
        public static Client ToDomain(AddClientRequest request)
        {
            Client client = new Client
            (
                request.Name,
                request.RUT,
                request.RazonSocial,
                request.Address,
                request.MapsAddress,
                request.Schedule,
                request.Phone,
                contactName: request.ContactName,
                request.Email,
                request.Observations,
                new List<BankAccount>(),
                loanedCrates: 0,
                request.Qualification,
                new Zone(request.ZoneId)
            );
            client.AuditInfo.SetCreated(request.getUserId(), request.Location);

            return client;
        }

        public static Client.UpdatableData ToUpdatableData(UpdateClientRequest request)
        {
            return new Client.UpdatableData
            {
                Name = request.Name,
                RUT = request.RUT,
                RazonSocial = request.RazonSocial,
                Address = request.Address,
                MapsAddress = request.MapsAddress,
                Schedule = request.Schedule,
                Phone = request.Phone,
                ContactName = request.ContactName,
                Email = request.Email,
                Observations = request.Observations,
                LoanedCrates = request.LoanedCrates,
                Qualification = request.Qualification,
                Zone = new Zone(request.ZoneId),
                BankAccounts = new List<BankAccount>(),
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static ClientResponse ToResponse(Client client)
        {
            return new ClientResponse
            {
                Id = client.Id,
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
                LoanedCrates = client.LoanedCrates,
                Qualification = client.Qualification,
                ZoneId = client.Zone.Id,
                AuditInfo = AuditMapper.ToResponse(client.AuditInfo)
            };
        }
    }
}
