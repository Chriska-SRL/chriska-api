using BusinessLogic.Dominio;
using BusinessLogic.DTOsPayment;
using BusinessLogic.DTOsPayments;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class PaymentsSubSystem
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly ISupplierRepository _supplierRepository;
        public PaymentsSubSystem(IPaymentRepository paymentRepository, ISupplierRepository supplierRepository)
        {
            _paymentRepository = paymentRepository;
            _supplierRepository = supplierRepository;
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
    }
}
