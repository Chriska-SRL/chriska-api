using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class PaymentsSubSystem
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsSubSystem(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public void AddPayment(AddPaymentRequest paymentRequest)
        {
            Payment newPayment = PaymentMapper.ToDomain(paymentRequest);
            newPayment.Validate();
            _paymentRepository.Add(newPayment);
        }

        public void UpdatePayment(UpdatePaymentRequest paymentRequest)
        {
            Payment existingPayment = _paymentRepository.GetById(paymentRequest.Id);
            if (existingPayment == null) throw new Exception("No se encontro el pago");
            existingPayment.Update(PaymentMapper.ToDomain(paymentRequest));
            _paymentRepository.Update(existingPayment);
        }
        public void DeletePayment(DeletePaymentRequest paymentRequest)
        {
            var payment = _paymentRepository.GetById(paymentRequest.Id);
            if (payment == null) throw new Exception("No se encontro el pago");
            _paymentRepository.Delete(paymentRequest.Id);
        }

        public PaymentResponse GetPaymentById(int id)
        {
            Payment payment = _paymentRepository.GetById(id);
            if (payment == null) throw new Exception("No se encontro el pago");
            PaymentResponse paymentResponse = PaymentMapper.ToResponse(payment);
            return paymentResponse;
        }
        public List<PaymentResponse> GetAllPayments()
        {
            List<Payment> payments = _paymentRepository.GetAll();
            if(!payments.Any())
                throw new Exception("No se encontraron pagos");
            return payments.Select(PaymentMapper.ToResponse).ToList();
        }
    }
}
