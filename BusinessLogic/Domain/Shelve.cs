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
        public Shelve(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "El ID del estante debe ser mayor a cero.");

            Id = id;
            Description = "-";
            Warehouse = new Warehouse(9999,"Temporal", "-", "-", new List<Shelve>());
            Stocks = new List<ProductStock>();
            StockMovements = new List<StockMovement>();
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
