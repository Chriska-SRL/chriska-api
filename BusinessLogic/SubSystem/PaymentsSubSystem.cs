using BusinessLogic.Dominio;
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
        public PaymentsSubSystem(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public void AddPayment(Payment payment)
        {
            _paymentRepository.Add(payment);
        }

    }
}
