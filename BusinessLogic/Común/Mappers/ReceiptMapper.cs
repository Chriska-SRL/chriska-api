using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.Común.Mappers
{
    public static class ReceiptMapper
    {
        public static Receipt ToDomain(AddReceiptRequest addReceiptRequest)
        {
            return new Receipt(

                id: 0,
                date: addReceiptRequest.Date,
                amount: addReceiptRequest.Amount,
                paymentMethod: addReceiptRequest.PaymentMethod,
                notes: addReceiptRequest.Notes,
                client: new Client(id : addReceiptRequest.ClientId)
            );
        }
        public static Receipt.UpdatableData ToUpdatableData(UpdateReceiptRequest updateReceiptRequest)
        {
            return new Receipt.UpdatableData
            {
                Date = updateReceiptRequest.Date,
                Amount = updateReceiptRequest.Amount,
                PaymentMethod = updateReceiptRequest.PaymentMethod,
                Notes = updateReceiptRequest.Notes,
                Client = new Client(id: updateReceiptRequest.ClientId)
            };
        }


        public static ReceiptResponse ToResponse(Receipt receipt)
        {
            return new ReceiptResponse
            {
                Id=receipt.Id,
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
                    Observations = receipt.Client.Observations,
                    LoanedCrates = receipt.Client.LoanedCrates
                }
            };
        }
    }
}
