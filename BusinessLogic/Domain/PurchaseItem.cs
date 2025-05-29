namespace BusinessLogic.Dominio
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; }

        public PurchaseItem(int quantity, decimal unitPrice, Product product)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            Product = product;
        }

        public void Validate()
        {

            if (Quantity <= 0) throw new Exception("La cantidad debe ser mayor a 0");
            if (UnitPrice <= 0) throw new Exception("El precio unitario debe ser mayor a 0");
            if (Product == null) throw new Exception("El producto es obligatorio");
        }

        public void Update(int quantity, decimal unitPrice)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
