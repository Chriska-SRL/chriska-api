using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class Order
    {
        private int OrderId { get; set; }
        private DateTime OrderDate { get; set; }
        private string ClientName { get; set; }
        private int Crates { get; set; }
        private string Status { get; set; }
        private Delivery Delivery { get; set; }
        private List<OrderItem> OrderItems { get; set; }
        private Sale Sale { get; set; } = new Sale();
        private User PreparedBy { get; set; } = new User();
        private User DeliveredBy { get; set; } = new User();


    }
}
