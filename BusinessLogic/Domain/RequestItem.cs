namespace BusinessLogic.Dominio
{
    public class RequestItem:IEntity<RequestItem.UpdatableData>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; } 

        public RequestItem(int id, int quantity, decimal unitPrice, Product product)
        {
            Id = id;
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
        public void Update(UpdatableData data) { 
            Quantity = data.Quantity;
            UnitPrice = data.UnitPrice;
            Validate();
        }
        public class UpdatableData
        {
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
