namespace BusinessLogic.Dominio
{
    public class Shelve : IEntity<Shelve.UpdatableData>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        public List<StockMovement> StockMovements { get; set; } = new List<StockMovement>();


        public Shelve(int id, string description, Warehouse warehouse, List<ProductStock> productStocks, List<StockMovement> stockMovements)
        {
            Id = id;
            Description = description;
            Warehouse = warehouse;
            Stocks = productStocks;
            StockMovements = stockMovements;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Description)) throw new Exception("La descripción es obligatoria");
            if (Warehouse == null) throw new Exception("El almacén es obligatorio");
        }

        public void Update(UpdatableData data)
        {
            Description = data.Description;
            Warehouse = data.Warehouse;
            Validate();
        }
        public class UpdatableData
        {
            public string Description { get; set; }
            public Warehouse Warehouse { get; set; }
        }
    }
}
