using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class DeliveriesSubSystem
    {
        private readonly IDeliveryRepository _deliveryRepository;
        public DeliveriesSubSystem(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        // Entregas

        public DeliveryResponse AddDelivery(AddDeliveryRequest request)
        {
            Delivery delivery = DeliveryMapper.ToDomain(request);
            delivery.Validate();

            Delivery added = _deliveryRepository.Add(delivery);
            return DeliveryMapper.ToResponse(added);
        }

        public DeliveryResponse UpdateDelivery(UpdateDeliveryRequest request)
        {
            Delivery existing = _deliveryRepository.GetById(request.Id)
                                  ?? throw new InvalidOperationException("Entrega no encontrada.");

            Delivery.UpdatableData updatedData = DeliveryMapper.ToUpdatableData(request);
            existing.Update(updatedData);

            Delivery updated = _deliveryRepository.Update(existing);
            return DeliveryMapper.ToResponse(updated);
        }

        public DeliveryResponse DeleteDelivery(DeleteDeliveryRequest request)
        {
            Delivery deleted = _deliveryRepository.Delete(request.Id)
                                  ?? throw new InvalidOperationException("Entrega no encontrada.");

            return DeliveryMapper.ToResponse(deleted);
        }

        public DeliveryResponse GetDeliveryById(int id)
        {
            Delivery delivery = _deliveryRepository.GetById(id)
                                   ?? throw new InvalidOperationException("Entrega no encontrada.");

            return DeliveryMapper.ToResponse(delivery);
        }

        public List<DeliveryResponse> GetAllDeliveries()
        {
            return _deliveryRepository.GetAll()
                                      .Select(DeliveryMapper.ToResponse)
                                      .ToList();
        }

    }
}
