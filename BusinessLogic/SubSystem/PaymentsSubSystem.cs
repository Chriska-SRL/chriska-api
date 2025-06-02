using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsPayment;

namespace BusinessLogic.SubSystem
{
    public class PaymentsSubSystem
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISupplierRepository _supplierRepository;

        private readonly SuppliersSubSystem _suppliersSubSystem;

        public PaymentsSubSystem(IPaymentRepository paymentRepository, ISupplierRepository supplierRepository, SuppliersSubSystem suppliersSubSystem)
        {
            _paymentRepository = paymentRepository;
            _supplierRepository = supplierRepository;

            _suppliersSubSystem = suppliersSubSystem;
        }

        public void AddPayment(AddPaymentRequest paymentRequest)
        {
            var payment = new Payment(paymentRequest.Date, paymentRequest.Amount, paymentRequest.PaymentMethod,paymentRequest.Note, _supplierRepository.GetById(paymentRequest.SupplierId));
            payment.Validate();
            _paymentRepository.Add(payment);
        }

        public void UpdatePayment(UpdatePaymentRequest paymentRequest)
        {
            var payment = _paymentRepository.GetById(paymentRequest.Id);
            if (payment == null) throw new Exception("No se encontro el pago");
            payment.Update(paymentRequest.Date, paymentRequest.Amount, paymentRequest.PaymentMethod, paymentRequest.Note, _supplierRepository.GetById(paymentRequest.SupplierId));
            _paymentRepository.Update(payment);
        }

        public PaymentResponse GetPaymentById(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) throw new Exception("No se encontro el pago");
            var paymentResponse = new PaymentResponse
            {
                Date = payment.Date,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Note = payment.Note,
                Supplier = _suppliersSubSystem.GetSupplierById(payment.Supplier.Id),
            };
            return paymentResponse;
        }

        public void DeletePayment(DeletePaymentRequest paymentRequest)
        {
            var payment = _paymentRepository.GetById(paymentRequest.Id);
            if (payment == null) throw new Exception("No se encontro el pago");
            _paymentRepository.Delete(paymentRequest.Id);
        }

        //public void GetAllPayments()
        //{
        //    var payments = _paymentRepository.GetAll();
        //    if (payments == null || !payments.Any()) throw new Exception("No se encontraron pagos");
        //    return payments;
        //}
    }
}
