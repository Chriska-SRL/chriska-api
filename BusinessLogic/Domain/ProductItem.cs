using BusinessLogic.Common;

namespace BusinessLogic.Domain

{
    public class ProductItem
    {
        public decimal Quantity { get; set; }
        public int? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public Product Product { get; set; }
        public AuditInfo? AuditInfo { get; set ; }

        public ProductItem(decimal quantity, int? weight, decimal unitPrice, decimal discount, Product product)
        {
            Quantity = quantity;
            Weight = weight;
            UnitPrice = unitPrice;
            Discount = discount;
            Product = product ?? throw new ArgumentNullException(nameof(product));
        }
        internal void Validate()
        {
                if (Quantity % 0.5m != 0)
                {
                    throw new InvalidOperationException(
                        $"El producto '{Product.Name}' tiene una cantidad inválida ({Quantity}). " +
                        "Solo se permiten múltiplos de 0.5 al preparar una orden."
                    );
                }

        }
    }
}
