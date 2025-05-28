using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dominio
{
    public class ReturnRequest: Request
    {
        public CreditNote CreditNote { get; set; }
        public ReturnRequest(CreditNote creditNote, DateTime requestDate, DateTime deliveryDate, string status, string observation, User user, Client client)
        {
            CreditNote = creditNote;
            RequestDate = requestDate;
            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
            User = user;
            Client = client;

        }

        public override void Validate()
        {
            if (CreditNote == null) throw new Exception("La nota de credito es obligatoria para una solicitud de devolucion");
            if(RequestDate==null) throw new Exception("La fecha de solicitud es obligatoria para una solicitud de devolucion");
            if (DeliveryDate == null) throw new Exception("La fecha de entrega es obligatoria para una solicitud de devolucion");
            if (string.IsNullOrEmpty(Status)) throw new Exception("El estado es obligatorio para una solicitud de devolucion");
            if (User == null) throw new Exception("El usuario es obligatorio para una solicitud de devolucion");
            if (Client == null) throw new Exception("El cliente es obligatorio para una solicitud de devolucion");


        }
        
        public override void Update(DateTime deliveryDate, string status,string observation, User user, Client client)
        {

            DeliveryDate = deliveryDate;
            Status = status;
            Observation = observation;
            User = user;
            Client = client;
        }
   
    }
}
