
namespace BusinessLogic.DTOs.DTOsReceipt
{
    public class AddReceiptRequest
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public int ClientId { get; set; }
    }
}
