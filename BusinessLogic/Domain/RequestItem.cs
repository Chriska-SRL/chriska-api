namespace BusinessLogic.Dominio
{
    public class RequestItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; } 
    }
}
