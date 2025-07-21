namespace BusinessLogic.Dominio
{
    public class OrderRequest : Request, IEntity<OrderRequest.UpdatableData>
    {
        public Order Order { get; set; }

        public override void Validate()
        {
            if (Order == null) throw new Exception("El pedido es obligatorio para una solicitud de devolucion");
            if (RequestDate == null) throw new Exception("La fecha de solicitud es obligatoria para una solicitud de devolucion");
            if (DeliveryDate == null) throw new Exception("La fecha de entrega es obligatoria para una solicitud de devolucion");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado es obligatorio para una solicitud de devolucion");
        }

        public OrderRequest(int id,Order order, DateTime requestDate, DateTime deliveryDate, string status, string observation)
        {
            Id = id;
            Order = order;
            RequestDate = requestDate;
            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
        }
        public OrderRequest(int id)
        {
            Id = id;
            //Order = order;
            //RequestDate = requestDate;
           // DeliveryDate = deliveryDate;
           // Status = status;
            //Observation = observation;
        }
        public void Update(UpdatableData data)
        {
            RequestDate = data.RequestDate;
            DeliveryDate = data.DeliveryDate;
            Status = data.Status;
            Observation = data.Observation;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime RequestDate { get; set; }
            public DateTime DeliveryDate { get; set; }
            public string Status { get; set; }
            public string Observation { get; set; }
        }
    }
}
