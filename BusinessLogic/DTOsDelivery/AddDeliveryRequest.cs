using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOsDelivery
{
    public class AddDeliveryRequest
    {
        private DateTime Date { get; set; }
        private string DriverName { get; set; }
        private string Observation { get; set; }
        private int CostId { get; set; } 

        private Vehicle Vehicle { get; set; }

    }
}
