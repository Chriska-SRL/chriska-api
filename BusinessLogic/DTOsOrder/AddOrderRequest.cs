using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsOrder
{
    public class AddOrderRequest
    {
        private DateTime Date { get; set; }
        private string ClientName { get; set; }
        private int Crates { get; set; }
        private string Status { get; set; }
        private int DeliveryId { get; set; }
        private Sale Sale { get; set; } = new Sale();
        private int PreparedById { get; set; } 
        private int DeliveredById { get; set; } 
        private int OrderRequestId { get; set; }
    }
}
