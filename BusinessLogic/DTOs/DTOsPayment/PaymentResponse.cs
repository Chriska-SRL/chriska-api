using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsPayment
{
    public class PaymentResponse
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public SupplierResponse Supplier { get; set; }
    }
}
