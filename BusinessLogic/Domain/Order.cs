using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class Order : IEntity<Order.UpdatableData>, IAuditable
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
        public OrderRequest OrderRequest { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Order(int id, DateTime date, string clientName, int crates, string status, Delivery delivery, Sale sale, User preparedBy, User deliveredBy, OrderRequest orderRequest, List<OrderItem> orderItems)
        {
            Id = id;
            Date = date;
            ClientName = clientName;
            Crates = crates;
            Status = status;
            Delivery = delivery;
            Sale = sale;
            PreparedBy = preparedBy;
            DeliveredBy = deliveredBy;
            OrderRequest = orderRequest;
            OrderItems = orderItems;
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

        public void Update(UpdatableData data)
        {
            Date = data.Date;
            ClientName = data.ClientName;
            Crates = data.Crates;
            Status = data.Status;
            Delivery = data.Delivery;
            Sale = data.Sale;
            PreparedBy = data.PreparedBy;
            DeliveredBy = data.DeliveredBy;
            OrderRequest = data.OrderRequest;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public string ClientName { get; set; }
            public int Crates { get; set; }
            public string Status { get; set; }
            public Delivery Delivery { get; set; }
            public Sale Sale { get; set; }
            public User PreparedBy { get; set; }
            public User DeliveredBy { get; set; }
            public OrderRequest OrderRequest { get; set; }
        }
    }
}
