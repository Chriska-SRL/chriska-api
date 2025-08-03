using BusinessLogic.DTOs.DTOsAudit;
using BusinessLogic.DTOs.DTOsShelve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsWarehouse
{
    public class WarehouseResponse:AuditableResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ShelveResponse> Shelves { get; set; } = new List<ShelveResponse>();
    }
}
