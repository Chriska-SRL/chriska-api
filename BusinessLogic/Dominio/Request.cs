using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public abstract class Request
    {
        private int RequestId { get; set; }
        private DateTime RequestDate { get; set; }
        private DateTime DeliveryDate { get; set; }
        private string Status { get; set; }
        private string Observation { get; set; }
        private User User { get; set; }
        private Client Client { get; set; }
        private List<RequestItem> RequestItems { get; set; } = new List<RequestItem>();
    }
}
