namespace BusinessLogic.Dominio
{
    public class Sale : IEntity<Sale.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public List<SaleItem> SaleItems = new List<SaleItem>();

        public Sale(int id, DateTime date, string status, List<SaleItem> saleItems)
        {
            Id = id;
            Date = date;
            Status = status;
            SaleItems = saleItems;
        }

        public Sale(int id)
        {
            Id = id;
           // Date = date;
           // Status = status;
           // SaleItems = saleItems;
        }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado de la venta no puede estar vacío");
            if (SaleItems.Count == 0) throw new Exception("La venta debe tener al menos un artículo");
        }

        public void Update(UpdatableData data)
        {
            Date = data.Date;
            Status = data.Status;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public string Status { get; set; }
        }
    }
}
