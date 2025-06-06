namespace BusinessLogic.Dominio
{
    public class ReturnRequest : Request, IEntity<ReturnRequest.UpdatableData>
    {
        public CreditNote CreditNote { get; set; }

        public ReturnRequest(int id, CreditNote creditNote, DateTime requestDate, DateTime deliveryDate, string status, string observation, User user, Client client, List<RequestItem> requestItems)
        {
            Id = id;
            CreditNote = creditNote;
            RequestDate = requestDate;
            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
            User = user;
            Client = client;
            RequestItems = requestItems;
        }

        public override void Validate()
        {
            if (CreditNote == null) throw new Exception("La nota de credito es obligatoria para una solicitud de devolucion");
            if (RequestDate == null) throw new Exception("La fecha de solicitud es obligatoria para una solicitud de devolucion");
            if (DeliveryDate == null) throw new Exception("La fecha de entrega es obligatoria para una solicitud de devolucion");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado es obligatorio para una solicitud de devolucion");
            if (User == null) throw new Exception("El usuario es obligatorio para una solicitud de devolucion");
            if (Client == null) throw new Exception("El cliente es obligatorio para una solicitud de devolucion");


        }

        public void Update(UpdatableData data)
        {
            DeliveryDate = data.DeliveryDate;
            Status = data.Status;
            Observation = data.Observation;
            User = data.User;
            Client = data.Client;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime DeliveryDate { get; set; }
            public string Status { get; set; }
            public string Observation { get; set; }
            public User User { get; set; }
            public Client Client { get; set; }
        }
    }
}
