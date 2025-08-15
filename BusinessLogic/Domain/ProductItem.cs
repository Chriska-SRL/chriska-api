namespace BusinessLogic.Domain

{
    public class ProductItem
    {
        public int Quantity { get; set; }
        public int? Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public Product Product { get; set; }



        public ProductItem(int quantity, int? weight, decimal unitPrice, decimal discount, Product product)
        {
            Quantity = quantity;
            Weight = weight;
            UnitPrice = unitPrice;
            Discount = discount;
            Product = product ?? throw new ArgumentNullException(nameof(product));
        }

    }
}
