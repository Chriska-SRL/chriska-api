using BusinessLogic.Común.Enums;

namespace BusinessLogic.DTOs.DTOsStockMovement
{
    public class AddStockMovementRequest
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public StockMovementType Type { get; set; }
        public string Reason { get; set; }
        public int ShelveId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
