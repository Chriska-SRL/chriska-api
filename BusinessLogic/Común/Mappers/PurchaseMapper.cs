using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                supplier: new Supplier
                (
                    id: purchaseRequest.SupplierId,
                    name: string.Empty,
                    rut: string.Empty,
                    razonSocial: string.Empty,
                    address: string.Empty,
                    mapsAddress: string.Empty,
                    phone: string.Empty,
                    contactName: string.Empty,
                    email: string.Empty,
                    bank: string.Empty,
                    bankAccount: string.Empty,
                    observations: string.Empty,
                    products: new List<Product>(),
                    payments: new List<Payment>(),
                    purchases: new List<Purchase>(),
                    daysToDeliver: new List<Day>()
                )
            );
        }
        public static Purchase.UpdatableData ToDomain(UpdatePurchaseRequest purchaseRequest)
        {
            return new Purchase.UpdatableData
            {
                Date = purchaseRequest.Date,
                Status = purchaseRequest.Status,
                Supplier = new Supplier
                (
                    id: purchaseRequest.SupplierId,
                    name: string.Empty,
                    rut: string.Empty,
                    razonSocial: string.Empty,
                    address: string.Empty,
                    mapsAddress: string.Empty,
                    phone: string.Empty,
                    contactName: string.Empty,
                    email: string.Empty,
                    bank: string.Empty,
                    bankAccount: string.Empty,
                    observations: string.Empty,
                    products: new List<Product>(),
                    payments: new List<Payment>(),
                    purchases: new List<Purchase>(),
                    daysToDeliver: new List<Day>()
                )
            };
        }
        public static PurchaseResponse toResponse(Purchase purchase)
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
                    Bank = purchase.Supplier.Bank,
                    BankAccount = purchase.Supplier.BankAccount,
                    Observations = purchase.Supplier.Observations
                }
            };
        }
    }
}
