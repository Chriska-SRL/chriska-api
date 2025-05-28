using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class OrderRequest : Request
    {
       public Order Order { get; set; }

  


            public override void Validate()
        {
            if (Order == null) throw new Exception("El pedido es obligatorio para una solicitud de devolucion");
            if (RequestDate == null) throw new Exception("La fecha de solicitud es obligatoria para una solicitud de devolucion");
            if (DeliveryDate == null) throw new Exception("La fecha de entrega es obligatoria para una solicitud de devolucion");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado es obligatorio para una solicitud de devolucion");
            if (User == null) throw new Exception("El usuario es obligatorio para una solicitud de devolucion");
            if (Client == null) throw new Exception("El cliente es obligatorio para una solicitud de devolucion");

        }

        public OrderRequest(Order order, DateTime requestDate, DateTime deliveryDate, string status, string observation, User user, Client client)
        {
            Order = order;
            RequestDate = requestDate;
            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
            User = user;
            Client = client;

        }
        public override void Update(DateTime deliveryDate, string status, string observation, User user, Client client)
        {

            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
            User = user;
            Client = client;
        }


    }
}
