using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.DTOsStockMovement
{
    public class StockMovementResponse
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public ShelveResponse Shelve { get; set; }
        public UserResponse User { get; set; }
        public ProductResponse Product { get; set; }
    }
}
