using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsClient;

namespace BusinessLogic.Common.Mappers
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
                request.Location,
                request.Schedule,
                request.Phone,
                contactName: request.ContactName,
                request.Email,
                request.Observations,
                bankAccounts: request.BankAccounts.Select(BankAccountMapper.ToDomain).ToList(),
                loanedCrates: 0,
                new Zone(request.ZoneId)
            );
            client.AuditInfo.SetCreated(request.getUserId(), request.AuditLocation);

            return client;
        }

        public static Client.UpdatableData ToUpdatableData(UpdateClientRequest request, Zone zone)
        {
            return new Client.UpdatableData
            {
                Name = request.Name,
                RUT = request.RUT,
                RazonSocial = request.RazonSocial,
                Address = request.Address,
                Location = request.Location,
                Schedule = request.Schedule,
                Phone = request.Phone,
                ContactName = request.ContactName,
                Email = request.Email,
                Observations = request.Observations,
                LoanedCrates = request.LoanedCrates,
                Qualification = request.Qualification,
                Zone = zone,
                BankAccounts = request.BankAccounts?.Select(BankAccountMapper.ToDomain).ToList(),
                UserId = request.getUserId()
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
                Location = client.Location,
                Schedule = client.Schedule,
                Phone = client.Phone,
                ContactName = client.ContactName,
                Email = client.Email,
                Observations = client.Observations,
                LoanedCrates = client.LoanedCrates,
                Qualification = client.Qualification,
                Zone = ZoneMapper.ToResponse(client.Zone),
                BankAccounts = client.BankAccounts.Select(BankAccountMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(client.AuditInfo)
            };
        }
    }
}
