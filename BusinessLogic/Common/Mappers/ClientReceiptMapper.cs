using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.Common.Mappers
{
    public static class ClientReceiptMapper
    {
        public static ClientReceipt ToDomain(ClientReceiptAddRequest request, Client client)
        {
            var receipt = new ClientReceipt
            (
                request.Date,
                request.Amount,
                request.Notes ?? string.Empty,
                request.PaymentMethod,
                client
            );
            receipt.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);

            return receipt;
        }

        public static ClientReceipt.UpdatableData ToUpdatableData(ReceiptUpdateRequest request)
        {
            return new ClientReceipt.UpdatableData
            {
                Notes = request.Notes,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static ClientReceiptResponse? ToResponse(ClientReceipt? receipt)
        {
            if (receipt == null) return null;

            return new ClientReceiptResponse
            {
                Id = receipt.Id,
                Date = receipt.Date,
                Amount = receipt.Amount,
                Notes = receipt.Notes,
                PaymentMethod = receipt.PaymentMethod,
                Client = ClientMapper.ToResponse(receipt.Client) ?? null,
                AuditInfo = AuditMapper.ToResponse(receipt.AuditInfo)
            };
        }
    }
}
