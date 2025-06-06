using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsCreditNote;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.Común.Mappers
{
    public static class ReturnRequestMapper
    {
        public static ReturnRequest ToDomain(AddReturnRequest_Request addReturnRequestRequest)
        {
            return new ReturnRequest(
                id: 0,
                creditNote: new CreditNote(
                    id: addReturnRequestRequest.CreditNoteId,
                    issueDate:DateTime.Now,
                    returnRequest:null
                ),
                requestDate: DateTime.Now,
                deliveryDate: addReturnRequestRequest.DeliveryDate,
                status: addReturnRequestRequest.Status,
                observation: addReturnRequestRequest.Observation,
                user: new User(
                    id: addReturnRequestRequest.UserId,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                client: new Client(
                    id: addReturnRequestRequest.ClientId,
                    name: string.Empty,
                    rut: string.Empty,
                    razonSocial: string.Empty,
                    address: string.Empty,
                    mapsAddress: string.Empty,
                    schedule: string.Empty,
                    phone: string.Empty,
                    contactName: string.Empty,
                    email: string.Empty,
                    observation: string.Empty,
                    bankAccount: string.Empty,
                    loanedCrates: 0,
                    zone: null,
                    requests: new List<Request>(),
                    receipts: new List<Receipt>()
                ),
                requestItems: new List<RequestItem>()
            );
        }
        public static ReturnRequest.UpdatableData ToDomain(UpdateReturnRequest_Request updateReturnRequestRequest)
        {
            return new ReturnRequest.UpdatableData
            {
                DeliveryDate = updateReturnRequestRequest.DeliveryDate,
                Status = updateReturnRequestRequest.Status,
                Observation = updateReturnRequestRequest.Observation,
                User = new User(
                    id: updateReturnRequestRequest.UserId,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),
                Client = new Client(
                    id: updateReturnRequestRequest.ClientId,
                    name: string.Empty,
                    rut: string.Empty,
                    razonSocial: string.Empty,
                    address: string.Empty,
                    mapsAddress: string.Empty,
                    schedule: string.Empty,
                    phone: string.Empty,
                    contactName: string.Empty,
                    email: string.Empty,
                    observation: string.Empty,
                    bankAccount: string.Empty,
                    loanedCrates: 0,
                    zone: null,
                    requests: new List<Request>(),
                    receipts: new List<Receipt>()
                )
            };
        }
        public static ReturnRequestResponse ToResponse(ReturnRequest returnRequest)
        {
            return new ReturnRequestResponse
            {
                CreditNote = new CreditNoteResponse
                {
                    Id = returnRequest.CreditNote.Id,
                    IssueDate = returnRequest.CreditNote.IssueDate
                },
                RequestDate = returnRequest.RequestDate,
                DeliveryDate = returnRequest.DeliveryDate,
                Status = returnRequest.Status,
                Observation = returnRequest.Observation,
                User = new UserResponse
                {
                    Id = returnRequest.User.Id,
                    Name = returnRequest.User.Name,
                    Username = returnRequest.User.Username
                },
                Client = new ClientResponse
                {
                    Id = returnRequest.Client.Id,
                    Name = returnRequest.Client.Name,
                    RUT = returnRequest.Client.RUT,
                    RazonSocial = returnRequest.Client.RazonSocial,
                    Address = returnRequest.Client.Address,
                    MapsAddress = returnRequest.Client.MapsAddress,
                    Schedule = returnRequest.Client.Schedule,
                    Phone = returnRequest.Client.Phone,
                    ContactName = returnRequest.Client.ContactName,
                    Email = returnRequest.Client.Email,
                    Observation = returnRequest.Client.Observation,
                    BankAccount = returnRequest.Client.BankAccount,
                    LoanedCrates = returnRequest.Client.LoanedCrates
                }
            };
        }
    }
}
