using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public abstract class BalanceItem
    {
        public int Id { get; set; }
        public int EntityId { get; set; } // ClientId or SupplierId
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DocumentType DocumentType { get; set; }


    }
}
