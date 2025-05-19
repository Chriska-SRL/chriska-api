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
        public  int OrderId { get; set; }
        public  DateTime OrderDate { get; set; }
        public  string ClientName { get; set; }
        public int Crates { get; set; }
        public  string Status { get; set; }
        public Delivery Delivery { get; set; }
        public List<OrderItem> OrderItems { get; set; } 
        public Sale Sale { get; set; } = new Sale();
        public User PreparedBy { get; set; } = new User();
        public User DeliveredBy { get; set; } = new User();


    }
}
