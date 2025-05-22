using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsPayments
{
    public class AddPaymentRequest
    {
        private DateTime Date { get; set; }
        private decimal Amount { get; set; }
        private string PaymentMethod { get; set; }
        private string Note { get; set; }
        private int SupplierId { get; set; }
    }
}
