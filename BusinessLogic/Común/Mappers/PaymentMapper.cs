using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.DTOs.DTOsSupplier;

namespace BusinessLogic.Común.Mappers
{
    public static class PaymentMapper
    {
        public static Payment ToDomain (AddPaymentRequest addPaymentRequest)
        {
            return new Payment
            (
                id:0,
                date: addPaymentRequest.Date,
                amount: addPaymentRequest.Amount,
                paymentMethod: addPaymentRequest.PaymentMethod,
                note: addPaymentRequest.Note,
                supplier: new Supplier(addPaymentRequest.SupplierId)
            );
        }
        public static Payment.UpdatableData ToUpdatableData(UpdatePaymentRequest updatablePaymentRequest)
        {
            return new Payment.UpdatableData
            {
                Date = updatablePaymentRequest.Date,
                Amount = updatablePaymentRequest.Amount,
                PaymentMethod = updatablePaymentRequest.PaymentMethod,
                Note = updatablePaymentRequest.Note
            };           
        }
        public static PaymentResponse ToResponse(Payment domain)
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
