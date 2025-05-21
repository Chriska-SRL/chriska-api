using BusinessLogic.DTOs.DTOsWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class ShelveResponse
    {
        public string Description { get; set; }
        public WarehouseResponse WarehouseResponse { get; set; }
    }
}
