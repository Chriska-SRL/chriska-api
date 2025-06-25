using BusinessLogic.Común.Enums;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsUser;
namespace BusinessLogic.DTOs.DTOsStockMovement
{
    public class StockMovementResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; }
        public ShelveResponse Shelve { get; set; }
        public UserResponse User { get; set; }
        public ProductResponse Product { get; set; }
    }
}
