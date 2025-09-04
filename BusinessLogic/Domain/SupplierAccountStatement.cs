namespace BusinessLogic.Domain
{
    public class SupplierAccountStatement:AccountStatement
    {
        public Supplier Supplier { get; set; }
        public SupplierAccountStatement(Supplier supplier, List<BalanceItem> items)
        {
            Supplier = supplier;
            BalanceItems = items ?? new List<BalanceItem>();
        }
    }
}
