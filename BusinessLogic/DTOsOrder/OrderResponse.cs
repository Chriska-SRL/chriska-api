using BusinessLogic.Dominio;
using BusinessLogic.DTOsDelivery;
using BusinessLogic.DTOsOrderItem;
using BusinessLogic.DTOsSale;
using BusinessLogic.DTOsUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsOrder
{
    public class OrderResponse
    {
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public int Crates { get; set; }
        public string Status { get; set; }
        public DeliveryResponse DeliveryId { get; set; }
        public SaleResponse SaleId { get; set; }
        public UserResponse PreparedById { get; set; }
        public UserResponse DeliveredById { get; set; }
        public OrderItemResponse OrderRequestId { get; set; }
        
    }
}
