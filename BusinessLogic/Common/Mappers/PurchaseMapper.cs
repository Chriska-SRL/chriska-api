using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsPurchase;

namespace BusinessLogic.Common.Mappers
{
    public static class PurchaseMapper
    {
        public static PurchaseResponse? ToResponse(Purchase? purchase)
        {
            if (purchase == null) return null;
            return new PurchaseResponse
            {
                Id = purchase.Id,
                Date = purchase.Date,
                Observations = purchase.Observations,
                InvoiceNumber = purchase.InvoiceNumber,
                Status = purchase.Status,
                Supplier = SupplierMapper.ToResponse(purchase.Supplier),
                ProductItems = purchase.ProductItems?.Select(ProductItemMapper.ToResponse).ToList(),
                AuditInfo = AuditMapper.ToResponse(purchase.AuditInfo)
            };
        }

        public static Purchase ToDomain(PurchaseAddRequest request, Supplier supplier, List<ProductItem> items, User user)
        {
            var purchase = new Purchase(
                observation: request.Observations,
                user: user,
                productItems: items,
                status: BusinessLogic.Common.Enums.Status.Pending,
                supplier: supplier,
                invoiceNumber: request.InvoiceNumber
            );
            purchase.AuditInfo.SetCreated(request.getUserId(), request.AuditLocation);
            return purchase;
        }

        public static Purchase.UpdatableData ToUpdatableData(PurchaseUpdateRequest request, List<ProductItem> items, Supplier? supplier)
        {
            return new Purchase.UpdatableData
            {
                Observations = request.Observations,
                InvoiceNumber = request.InvoiceNumber,
                ProductItems = items,
                Supplier = supplier,
                UserId = request.getUserId(),
                Location = request.AuditLocation
            };
        }
    }
}