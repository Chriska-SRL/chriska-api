using BusinessLogic.Domain;

namespace BusinessLogic.Common.Enums
{
    public enum DocumentType
    {
        ClientPayment,
        Purchase,
        Sale,
        SupplierPayment,
        ReturnSale,
        ReturnPurchase
    }

    public static class DocumentTypeMapper
    {
        public static DocumentType FromType(Type documentType)
        {
            if (documentType == typeof(ClientReceipt))
                return DocumentType.ClientPayment;
            else if (documentType == typeof(Purchase))
                return DocumentType.Purchase;
            else if (documentType == typeof(Delivery))
                return DocumentType.Sale;
            else if (documentType == typeof(SupplierReceipt))
                return DocumentType.SupplierPayment;
            else if (documentType == typeof(ReturnRequest))
                return DocumentType.ReturnSale;
            else if (documentType == typeof(ReturnPurchase))
                return DocumentType.ReturnPurchase;
            else
                throw new ArgumentException("Tipo de documento no reconocido", nameof(documentType));
        }
    }
}
