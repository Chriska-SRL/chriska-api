namespace BusinessLogic.Dominio
{
    public class ProductStock
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
