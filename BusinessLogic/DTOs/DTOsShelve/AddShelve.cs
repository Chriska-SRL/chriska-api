using BusinessLogic.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsShelve
{
    public class AddShelve
    {
        public string Description { get; set; }
        public int WarehouseId { get; set; }
    }
}
