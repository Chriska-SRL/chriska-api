using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.DTOs.DTOsZone;

namespace BusinessLogic.Común.Mappers
{
    public static class OrderRequestMapper
    {
        public static OrderRequest ToDomain(AddOrderRequest_Request orderRequest)
        {

            return new OrderRequest(

                id: 0,
                order: null,
                requestDate: orderRequest.RequestDate,
                deliveryDate: orderRequest.DeliveryDate,
                status: orderRequest.Status,
                observation: "",
                user: new User(
                    id: orderRequest.UserId,
                    name: "",
                    username: "",
                    password: "",
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
                ),                 
                client: new Client(
                    id: orderRequest.ClientId,
                    name: "",
                    rut: "",
                    razonSocial: "",
                    address: "",
                    mapsAddress: "",
                    schedule: "",
                    phone: "",
                    contactName: "",
                    email: "",
                    observation: "",
                    bankAccount: "",
                    loanedCrates: 0,
                    zone: null,
                    receipts: new List<Receipt>(),
                    requests: new List<Request>()
                ),
                requestsItems: new List<RequestItem>()
            );
        }
        public static OrderRequest.UpdatableData ToDomain(UpdateOrderRequest_Request updateOrderRequest)
        {
            return new OrderRequest.UpdatableData
            {
                RequestDate = updateOrderRequest.RequestDate,
                DeliveryDate = updateOrderRequest.DeliveryDate,
                Status = updateOrderRequest.Status,
                Observation = updateOrderRequest.Observation,
                User = new User
               (
                    id: updateOrderRequest.UserId,
                    name: "",
                    username: "",
                    password: "",
                    isEnabled: true,
                    role: null,
                    requests: new List<Request>()
               ),
                Client = new Client
                (
                    id: updateOrderRequest.ClientId,
                    name: "",
                    rut: "",
                    razonSocial: "",
                    address: "",
                    mapsAddress: "",
                    schedule: "",
                    phone: "",
                    contactName: "",
                    email: "",
                    observation: "",
                    bankAccount: "",
                    loanedCrates: 0,
                    zone: null,
                    receipts: new List<Receipt>(),
                    requests: new List<Request>()
                )
            };
        }
        public static OrderRequestResponse toResponse(OrderRequest orderRequest)
        {
            return new OrderRequestResponse
            {
                RequestDate = orderRequest.RequestDate,
                DeliveryDate = orderRequest.DeliveryDate,
                Status = orderRequest.Status,
                Observation = orderRequest.Observation,
                User = new UserResponse
                {
                    Name = orderRequest.User.Name,
                    Username = orderRequest.User.Username,
                    IsEnabled = orderRequest.User.isEnabled,
                    Role = new RoleResponse
                    {
                        Name = orderRequest.User.Role.Name
                    }
                },
                Client = new ClientResponse
                {
                    Id = orderRequest.Client.Id,
                    Name = orderRequest.Client.Name,
                    RUT = orderRequest.Client.RUT,
                    RazonSocial = orderRequest.Client.RazonSocial,
                    Address = orderRequest.Client.Address,
                    MapsAddress = orderRequest.Client.MapsAddress,
                    Schedule = orderRequest.Client.Schedule,
                    Phone = orderRequest.Client.Phone,
                    ContactName = orderRequest.Client.ContactName,
                    Email = orderRequest.Client.Email,
                    Observation = orderRequest.Client.Observation,
                    BankAccount = orderRequest.Client.BankAccount,
                    LoanedCrates = orderRequest.Client.LoanedCrates,
                    zone = new ZoneResponse
                    {
                        Id = orderRequest.Client.Zone.Id,
                        Name = orderRequest.Client.Zone.Name,
                        Description = orderRequest.Client.Zone.Description
                    }
                }
            };
        }
    }
}
