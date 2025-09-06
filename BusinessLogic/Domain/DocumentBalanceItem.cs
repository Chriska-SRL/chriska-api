using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class DocumentBalanceItem : BalanceItem
    {
        public DocumentBalanceItem(int id, int entityId, DateTime date, string description, decimal amount, decimal balance, DocumentType documentType)
        {
            Id = id;
            EntityId = entityId;
            Date = date;
            Description = description;
            Amount = amount;
            Balance = balance;
            DocumentType = documentType;
            Document = null;
        }

        public DocumentBalanceItem(ProductDocument document, AccountStatement accountStatement)
        {
            DocumentType type = DocumentTypeMapper.FromType(document.GetType());
            decimal amount = document.getAmount();
            if (type == DocumentType.ReturnSale)
                amount = -amount;

            EntityId = document.Id;
            Date = document.Date;
            Description = $"Pago N° {document.Id}";
            Amount = amount;
            Balance = accountStatement.getBalance() + amount;
            DocumentType = type;
            DocumentId = document.Id;
            Document = document;
        }

        public ProductDocument? Document { get; set; }
    }
}
