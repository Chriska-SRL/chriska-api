using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsReturnRequest;

namespace BusinessLogic.Común.Mappers
{
    public static class ReturnRequestMapper
    {
        public static ReturnRequest ToDomain(AddReturnRequest_Request addReturnRequestRequest)
        {
            return new ReturnRequest(
                id: 0,
                creditNote: new CreditNote(addReturnRequestRequest.CreditNoteId),
                requestDate: DateTime.Now,
                deliveryDate: addReturnRequestRequest.DeliveryDate,
                status: addReturnRequestRequest.Status,
                observation: addReturnRequestRequest.Observation,
                user: new User(addReturnRequestRequest.UserId),
                client: new Client(addReturnRequestRequest.ClientId),
                requestItems: new List<RequestItem>()
            );
        }
        public static ReturnRequest.UpdatableData ToUpdatableData(UpdateReturnRequest_Request updateReturnRequestRequest)
        {
            return new ReturnRequest.UpdatableData
            {
                DeliveryDate = updateReturnRequestRequest.DeliveryDate,
                Status = updateReturnRequestRequest.Status,
                Observation = updateReturnRequestRequest.Observation,
                User = new User(updateReturnRequestRequest.UserId),
                Client = new Client(updateReturnRequestRequest.ClientId)
            };
        }
        public static ReturnRequestResponse ToResponse(ReturnRequest returnRequest)
        {
            return new ReturnRequestResponse
            {
                CreditNote = CreditNoteMapper.ToResponse(returnRequest.CreditNote),
                RequestDate = returnRequest.RequestDate,
                DeliveryDate = returnRequest.DeliveryDate,
                Status = returnRequest.Status,
                Observation = returnRequest.Observation,
                User = UserMapper.ToResponse(returnRequest.User),
                Client = ClientMapper.ToResponse(returnRequest.Client),
            };
        }
    }
}
