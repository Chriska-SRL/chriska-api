using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Receipt
    {
        private int Id { get; set; }
        private DateTime Date { get; set; }
        private decimal Amount { get; set; }
        private string PaymentMethod { get; set; }
        private string Notes { get; set; }
        private Client Client { get; set; }
    }
}
