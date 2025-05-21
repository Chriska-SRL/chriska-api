using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsSale;
using BusinessLogic.DTOs.DTOsUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsOrder
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public int Crates { get; set; }
        public string Status { get; set; }
        public DeliveryResponse Delivery { get; set; }
        public SaleResponse Sale { get; set; }
        public UserResponse PreparedBy { get; set; }
        public UserResponse DeliveredBy { get; set; }
        public OrderRequestResponse OrderRequest { get; set; }
        
    }
}
