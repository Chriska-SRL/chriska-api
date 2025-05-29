namespace BusinessLogic.Dominio
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public int Crates { get; set; }
        public string Status { get; set; }
        public Delivery Delivery { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Sale Sale { get; set; } 
        public User PreparedBy { get; set; } 
        public User DeliveredBy { get; set; } 
        public Request OrderRequest { get; set; } 
        
        public Order (DateTime date, string clientName, int crates, string status, Delivery delivery, Sale sale, User preparedBy, User deliveredBy, Request orderRequest)
        {
            Date = date;
            ClientName = clientName;
            Crates = crates;
            Status = status;
            Delivery = delivery;
            Sale = sale;
            PreparedBy = preparedBy;
            DeliveredBy = deliveredBy;
            OrderRequest = orderRequest;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ClientName)) throw new Exception("El nombre del cliente no puede estar vacío");
            if (Crates <= 0) throw new Exception("La cantidad de cajas debe ser mayor a cero");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado no puede estar vacío");
            if (Delivery == null) throw new Exception("La entrega no puede estar vacía");
            if (OrderItems == null || OrderItems.Count == 0) throw new Exception("Los items de la orden no pueden estar vacíos");
            if (PreparedBy == null) throw new Exception("El usuario que preparó la orden no puede estar vacío");
            if (DeliveredBy == null) throw new Exception("El usuario que entregó la orden no puede estar vacío");
        }

        public void Update(string clientName,int crates,string status,Delivery delivery)
        {
            ClientName = clientName;
            Crates = crates;
            Status = status;
            Delivery = delivery;
        }
    }
}
