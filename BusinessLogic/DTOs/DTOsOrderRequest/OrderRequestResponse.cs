using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsOrderRequest
{
    public class OrderRequestResponse
    {
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public UserResponse User { get; set; }
        public ClientResponse Client { get; set; }

    }
}
