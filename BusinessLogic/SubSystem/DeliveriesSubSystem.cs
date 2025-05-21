using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class DeliveriesSubSystem
    {
        // Guía temporal: entidades que maneja este subsistema

        private List<Delivery> Deliveries = new List<Delivery>();

        private IDeliveryRepository _deliveryRepository;
        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }
    }
}
