using BusinessLogic.DTOs.DTOsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class ReceiptResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public ClientResponse Client { get; set; }
    }
}
