namespace BusinessLogic.Dominio
{
    public class ProductStock:IEntity<ProductStock.UpdatableData>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public ProductStock(int id, int quantity, Product product)
        {
            Id = id;
            Quantity = quantity;
            Product = product;
        }
        public void Validate()
        {
            if (Product == null) throw new Exception("El producto es obligatorio");
            if (Quantity < 0) throw new Exception("La cantidad no puede ser negativa");
        }
        public void Update(UpdatableData data)
        {
            Quantity = data.Quantity;
            Product = data.Product;
            Validate();
        }
        public class UpdatableData
        {
            public int Quantity { get; set; }
            public Product Product { get; set; }
        }
    }
}
