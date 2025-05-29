namespace BusinessLogic.Dominio
{
    public class Shelve
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
