using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsReturnRequest
{
    public class UpdateReturnRequest
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }


    }
}
