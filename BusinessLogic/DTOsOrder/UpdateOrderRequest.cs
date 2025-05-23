using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsOrder
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int Crates { get; set; }
        public string Status { get; set; }
        public int DeliveryId { get; set; }

    }
}
