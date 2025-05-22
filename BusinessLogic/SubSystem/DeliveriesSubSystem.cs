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

        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IVehicleRepository _vehicleRepository;
        //private IZoneRepository _zoneRepository;
        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository, IVehicleRepository vehicleRepository)
        {
            _deliveryRepository = deliveryRepository;
            _vehicleRepository = vehicleRepository;
        }
        public void AddDelivery(Delivery delivery)
        {
            _deliveryRepository.Add(delivery);
        }
        public void AddVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Add(vehicle);
        }

    }
}
