namespace BusinessLogic.Dominio
{
    public class Shelve
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public Shelve(string description, Warehouse warehouse)
        {
            Description = description;
            Warehouse = warehouse;
        }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción no puede estar vacía.");
            if (Warehouse == null)
                throw new ArgumentException("El almacén no puede ser nulo.");
        }
        public void Update(string description, Warehouse warehouse)
        {
            Description = description;
            Warehouse = warehouse;
        }
    }
}
