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

        private IDeliveryRepository _deliveryRepository;
        //private IZoneRepository _zoneRepository;
        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }
        public void AddDelivery(Delivery delivery)
        {
            _deliveryRepository.Add(delivery);
        }
    }
}
