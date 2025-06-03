using BusinessLogic.DTOs.DTOsSaleItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsSale
{
    public class SaleResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public List<SaleItemResponse> SaleItems { get; set; } = new List<SaleItemResponse>();
    }
}
