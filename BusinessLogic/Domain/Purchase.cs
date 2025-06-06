namespace BusinessLogic.Dominio
{
    public class Purchase:IEntity<Purchase.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public Supplier Supplier { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public Purchase(int id,DateTime date, string status, Supplier supplier)
        {
            Id = id;
            Date = date;
            Status = status;
            Supplier = supplier;
        }

        public void Validate()
        {
            if (Date == null) throw new Exception("La fecha es obligatoria");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado es obligatorio");
            if (Supplier == null) throw new Exception("El proveedor es obligatorio");
        }

        public void Update(UpdatableData data)
        {
            Date = data.Date;
            Status = data.Status;
            Supplier = data.Supplier;
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public string Status { get; set; }
            public Supplier Supplier { get; set; }
        }
    }
}
