namespace BusinessLogic.Dominio
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        List<SaleItem> SaleItems = new List<SaleItem>();

        public Sale(DateTime dateTime, string status)
        {
            Date = dateTime;
            Status = status;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado de la venta no puede estar vacío");
            if (SaleItems.Count == 0) throw new Exception("La venta debe tener al menos un artículo");
        }

        public void Update(DateTime dateTime, string status)
        {
            Date = dateTime;
            Status = status;
        }
    }
}
