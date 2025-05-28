using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsOrder
{
    public class AddOrderRequest
    {
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public int Crates { get; set; }
        public string Status { get; set; }
        public int DeliveryId { get; set; }
        public int SaleId { get; set; } 
        public int PreparedById { get; set; } 
        public int DeliveredById { get; set; } 
        public int OrderRequestId { get; set; }

    }
}
