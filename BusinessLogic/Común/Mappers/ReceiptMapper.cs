using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.Común.Mappers
{
    public static class ReceiptMapper
    {
        public static Receipt toDomain(AddReceiptRequest addReceiptRequest)
        {
            return new Receipt(

                id: 0,
                date: addReceiptRequest.Date,
                amount: addReceiptRequest.Amount,
                paymentMethod: addReceiptRequest.PaymentMethod,
                notes: addReceiptRequest.Notes,
                client: new Client
                (
                    id : addReceiptRequest.ClientId,
                    name : "",
                    rut : "",
                    razonSocial : "",
                    address : "",
                    mapsAddress : "",
                    schedule : "",
                    phone : "",
                    contactName : "",
                    email : "",
                    observation : "",
                    bankAccount :"",
                    loanedCrates : 0,
                    zone : null,
                    receipts : new List<Receipt>(),
                    requests : new List<Request>()
                )
            );
        }
        public static Receipt.UpdatableData toDomain(UpdateReceiptRequest updateReceiptRequest)
        {
            return new Receipt.UpdatableData
            {
                Date = updateReceiptRequest.Date,
                Amount = updateReceiptRequest.Amount,
                PaymentMethod = updateReceiptRequest.PaymentMethod,
                Notes = updateReceiptRequest.Notes,
                Client = new Client
                (
                    id: updateReceiptRequest.ClientId,
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


        public static ReceiptResponse toResponse(Receipt receipt)
        {
            return new ReceiptResponse
            {
                Date = receipt.Date,
                Amount = receipt.Amount,
                PaymentMethod = receipt.PaymentMethod,
                Notes = receipt.Notes,
                Client = new ClientResponse
                {
                    Id = receipt.Client.Id,
                    Name = receipt.Client.Name,
                    RUT = receipt.Client.RUT,
                    RazonSocial = receipt.Client.RazonSocial,
                    Address = receipt.Client.Address,
                    MapsAddress = receipt.Client.MapsAddress,
                    Schedule = receipt.Client.Schedule,
                    Phone = receipt.Client.Phone,
                    ContactName = receipt.Client.ContactName,
                    Email = receipt.Client.Email,
                    Observation = receipt.Client.Observation,
                    BankAccount = receipt.Client.BankAccount,
                    LoanedCrates = receipt.Client.LoanedCrates
                }
            };
        }
    }
}
