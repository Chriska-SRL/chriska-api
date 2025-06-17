using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class PaymentsSubSystem
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsSubSystem(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public PaymentResponse AddPayment(AddPaymentRequest request)
        {
            Payment payment = PaymentMapper.ToDomain(request);
            payment.Validate();

            Payment added = _paymentRepository.Add(payment);
            return PaymentMapper.ToResponse(added);
        }

        public PaymentResponse UpdatePayment(UpdatePaymentRequest request)
        {
            Payment existing = _paymentRepository.GetById(request.Id)
                                  ?? throw new InvalidOperationException("Pago no encontrado.");

            Payment.UpdatableData updatedData = PaymentMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Payment updated = _paymentRepository.Update(existing);
            return PaymentMapper.ToResponse(updated);
        }

        public PaymentResponse DeletePayment(DeletePaymentRequest request)
        {
            Payment deleted = _paymentRepository.Delete(request.Id)
                                  ?? throw new InvalidOperationException("Pago no encontrado.");

            return PaymentMapper.ToResponse(deleted);
        }

        public PaymentResponse GetPaymentById(int id)
        {
            Payment payment = _paymentRepository.GetById(id)
                                 ?? throw new InvalidOperationException("Pago no encontrado.");

            return PaymentMapper.ToResponse(payment);
        }

        public List<PaymentResponse> GetAllPayments()
        {
            return _paymentRepository.GetAll()
                                     .Select(PaymentMapper.ToResponse)
                                     .ToList();
        }
    }
}
