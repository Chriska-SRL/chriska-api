using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOsCategory;
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
        public void AddDelivery(AddDeliveryRequest deliveryRequest)
        {
            var delivery = new Delivery(deliveryRequest.Date, deliveryRequest.DriverName, deliveryRequest.Observation,_vehicleRepository.GetById(deliveryRequest.VehicleId));
            delivery.Validate();
            _deliveryRepository.Add(delivery);

        }

        public void UpdateDelivery(UpdateDeliveryRequest deliveryRequest)
        {
            var delivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            delivery.Update(deliveryRequest.DriverName, deliveryRequest.Observation, _vehicleRepository.GetById(deliveryRequest.VehicleId));
            _deliveryRepository.Update(delivery);
        }
        public void DeleteDelivery(DeleteDeliveryRequest deliveryRequest)
        {
            var delivery = _deliveryRepository.GetById(deliveryRequest.Id);
            if (delivery == null) throw new Exception("No se encontro la entrega");
            _deliveryRepository.Delete(deliveryRequest.Id);
        }
        //public List<Delivery> GetAllDeliveries()
        //{
        //    return _deliveryRepository.GetAll();
        //}

        //Agregar un subsistema de veiculos???
        public void AddVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Add(vehicle);
        }

        internal DeliveryResponse GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
