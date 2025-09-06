using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class ReceiptBalanceItem : BalanceItem
    {
        public ReceiptBalanceItem(Receipt receipt, AccountStatement accountStatement)
        {
            DocumentType type = DocumentTypeMapper.FromType(receipt.GetType());
            decimal amount = receipt.Amount;
            if (type == DocumentType.ClientPayment)
                amount = -amount;

            EntityId = receipt.Id;
            Date = receipt.Date;
            Description = $"Recibo N° {receipt.Id}";
            Amount = amount;
            Balance = accountStatement.getBalance() + amount;
            DocumentType = type;
            Receipt = receipt;
            DocumentId = receipt.Id;
        }

        public ReceiptBalanceItem(int id, int entityId, DateTime date, string description, decimal amount, decimal balance, DocumentType documentType)
        {
            Id = id;
            EntityId = entityId;
            Date = date;
            Description = description;
            Amount = amount;
            Balance = balance;
            DocumentType = documentType;
            Receipt = null;
        }

        public Receipt? Receipt { get; set; }
    }
}
