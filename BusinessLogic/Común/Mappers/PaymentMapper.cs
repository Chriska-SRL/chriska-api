using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class PaymentMapper
    {
        public static Payment toDomain (AddPaymentRequest addPaymentRequest)
        {
            return new Payment
            (
                id:0,
                date: addPaymentRequest.Date,
                amount: addPaymentRequest.Amount,
                paymentMethod: addPaymentRequest.PaymentMethod,
                note: addPaymentRequest.Note,
                supplier: new Supplier
                (
                    id: addPaymentRequest.SupplierId,
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
        public static Payment.UpdatableData toDomain(UpdatePaymentRequest updatablePaymentRequest)
        {
            return new Payment.UpdatableData
            {
                Date = updatablePaymentRequest.Date,
                Amount = updatablePaymentRequest.Amount,
                PaymentMethod = updatablePaymentRequest.PaymentMethod,
                Note = updatablePaymentRequest.Note,
                Supplier = new Supplier
                (
                    id: updatablePaymentRequest.SupplierId,
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
        public static PaymentResponse toResponse(Payment domain)
        {
            return new PaymentResponse
            {
                Date = domain.Date,
                Amount = domain.Amount,
                PaymentMethod = domain.PaymentMethod,
                Note = domain.Note,
                Supplier = new SupplierResponse
                {
                    Id = domain.Supplier.Id,
                    Name = domain.Supplier.Name,
                    RUT = domain.Supplier.RUT,
                    RazonSocial = domain.Supplier.RazonSocial,
                    Address = domain.Supplier.Address,
                    MapsAddress = domain.Supplier.MapsAddress,
                    Phone = domain.Supplier.Phone,
                    ContactName = domain.Supplier.ContactName,
                    Email = domain.Supplier.Email,
                    BankAccount = domain.Supplier.BankAccount,
                    Observations = domain.Supplier.Observations
                }
            };
        }
    }
}
