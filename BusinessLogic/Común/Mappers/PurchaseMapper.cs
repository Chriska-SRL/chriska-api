using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class PurchaseMapper
    {
        public static Purchase ToDomain(AddPurchaseRequest purchaseRequest)
        {
            return new Purchase
            (
                id: 0,
                date: purchaseRequest.Date,
                status: purchaseRequest.Status,
                supplier: new Supplier(purchaseRequest.SupplierId)
            );
        }
        public static Purchase.UpdatableData ToUpdatableData(UpdatePurchaseRequest purchaseRequest)
        {
            return new Purchase.UpdatableData
            {
                Date = purchaseRequest.Date,
                Status = purchaseRequest.Status
            };
        }
        public static PurchaseResponse ToResponse(Purchase purchase)
        {
            return new PurchaseResponse
            {
                Date = purchase.Date,
                Status = purchase.Status,
                Supplier = new SupplierResponse {
                    Id = purchase.Supplier.Id,
                    Name = purchase.Supplier.Name,
                    RUT = purchase.Supplier.RUT,
                    RazonSocial = purchase.Supplier.RazonSocial,
                    Address = purchase.Supplier.Address,
                    MapsAddress = purchase.Supplier.MapsAddress,
                    Phone = purchase.Supplier.Phone,
                    ContactName = purchase.Supplier.ContactName,
                    Email = purchase.Supplier.Email,
                    BankAccount = purchase.Supplier.BankAccount,
                    Observations = purchase.Supplier.Observations
                }
            };
        }
    }
}
