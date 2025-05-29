namespace BusinessLogic.Dominio
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; }

        public OrderItem(int quantity, decimal unitPrice, Product product)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            Product = product;
        }

        public void Validate()
        {
            if(Quantity <= 0) throw new Exception("La cantidad debe ser mayor a cero");
            if (UnitPrice <= 0) throw new Exception("El precio unitario debe ser mayor a cero");
            if (Product == null) throw new Exception("El producto no puede estar vacío");
        } 
    }
}
