using BusinessLogic.DTOs.DTOsShelve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsWarehouse
{
    public class WarehouseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public List<ShelveResponse> Shelves { get; set; } = new List<ShelveResponse>();
    }
}
