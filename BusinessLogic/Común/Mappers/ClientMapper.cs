using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;

namespace BusinessLogic.Común.Mappers
{
    public static class ClientMapper
    {
        public static Client ToDomain(AddClientRequest request)
        {
            return new Client(
                id: 0,
                name: request.Name,
                rut: request.RUT,
                razonSocial: request.RazonSocial,
                address: request.Address,
                mapsAddress: request.MapsAddress,
                schedule: request.Schedule,
                phone: request.Phone,
                contactName: request.ContactName,
                email: request.Email,
                observations: request.Observations,
                bankAccounts: new List<BankAccount>(),
                loanedCrates: 0,
                qualification: request.Qualification,
                zone: new Zone(request.ZoneId),
                auditInfo: AuditMapper.ToDomain(request.AuditInfo)
            );
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
                AuditInfo = AuditMapper.ToDomain(request.AuditInfo)
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
