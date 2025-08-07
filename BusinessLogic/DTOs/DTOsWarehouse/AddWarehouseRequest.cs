using BusinessLogic.DTOs.DTOsAudit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsWarehouse
{
    public class AddWarehouseRequest:AuditableRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
