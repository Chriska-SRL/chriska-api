using System.Reflection.PortableExecutable;

namespace BusinessLogic.Dominio
{
    public class ProductStock : IEntity<ProductStock.UpdatableData>
    {
        public int Id { get; set; } 
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public Shelve Shelve { get; set; }

        public ProductStock(int id, int quantity, Product product, Shelve shelve)
        {
            Id = id;
            Quantity = quantity;
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Shelve = shelve ?? throw new ArgumentNullException(nameof(shelve));
            Validate();
        }

        public void Validate()
        {
            if (Product == null)
                throw new ArgumentNullException(nameof(Product), "El producto es obligatorio.");

            if (Shelve == null)
                throw new ArgumentNullException(nameof(Shelve), "El estante es obligatorio.");

            if (Quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(Quantity), "La cantidad no puede ser negativa.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Quantity = data.Quantity ?? Quantity;
            Product = data.Product ?? Product;
            Shelve = data.Shelve ?? Shelve;

            Validate();
        }

        public class UpdatableData
        {
            public int? Quantity { get; set; }
            public Product? Product { get; set; }
            public Shelve? Shelve { get; set; }
        }

        public override string ToString()
        {
            return $"ProductStock(Id: {Id}, Quantity: {Quantity}, ProductId: {Product?.Id}, ShelveId: {Shelve?.Id})";
        }
    }
}
