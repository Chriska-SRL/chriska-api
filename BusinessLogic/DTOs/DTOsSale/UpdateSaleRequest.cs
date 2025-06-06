using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsSale
{
    public class UpdateSaleRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
