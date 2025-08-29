using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.Common.Mappers
{
    public static class ReceiptMapper
    {
        public static Receipt ToDomain(ReceiptAddRequest request, Client client)
        {
            var receipt = new Receipt
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

        public static Receipt.UpdatableData ToUpdatableData(ReceiptUpdateRequest request)
        {
            return new Receipt.UpdatableData
            {
                Notes = request.Notes,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static ReceiptResponse? ToResponse(Receipt? receipt)
        {
            if (receipt == null) return null;

            return new ReceiptResponse
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
