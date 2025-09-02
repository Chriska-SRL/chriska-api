using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsReceipt;

namespace BusinessLogic.Common.Mappers
{
    public static class SupplierReceiptMapper
    {
        public static SupplierReceipt ToDomain(SupplierReceiptAddRequest request, Supplier supplier)
        {
            var receipt = new SupplierReceipt
            (
                request.Date,
                request.Amount,
                request.Notes ?? string.Empty,
                request.PaymentMethod,
                supplier
            );
            receipt.AuditInfo?.SetCreated(request.getUserId(), request.AuditLocation);

            return receipt;
        }

        public static SupplierReceipt.UpdatableData ToUpdatableData(ReceiptUpdateRequest request)
        {
            return new SupplierReceipt.UpdatableData
            {
                Notes = request.Notes,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }

        public static SupplierReceiptResponse? ToResponse(SupplierReceipt? receipt)
        {
            if (receipt == null) return null;

            return new SupplierReceiptResponse
            {
                Id = receipt.Id,
                Date = receipt.Date,
                Amount = receipt.Amount,
                Notes = receipt.Notes,
                PaymentMethod = receipt.PaymentMethod,
                Supplier = SupplierMapper.ToResponse(receipt.Supplier) ?? null,
                AuditInfo = AuditMapper.ToResponse(receipt.AuditInfo)
            };
        }
    }
}
