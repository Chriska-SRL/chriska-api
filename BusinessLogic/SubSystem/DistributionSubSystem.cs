using BusinessLogic.Common.Mappers;
using BusinessLogic.Common;
using BusinessLogic.DTOs.DTOsDistribution;
using BusinessLogic.DTOs;
using BusinessLogic.Repository;
using BusinessLogic.Domain;

namespace BusinessLogic.SubSystem
{
    public class DistributionSubSystem
    {
        private readonly IDistributionRepository _distributionRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public DistributionSubSystem(IDistributionRepository distributionRepository, IZoneRepository zoneRepository, IVehicleRepository vehicleRepository, IUserRepository userRepository, IDeliveryRepository deliveryRepository)
        {
            _distributionRepository = distributionRepository;
            _zoneRepository = zoneRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task<DistributionResponse> AddDistributionAsync(DistributionAddRequest request)
        {
            Vehicle vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId)
                ?? throw new ArgumentException("No se encontró el vehículo seleccionado.");
            User user = await _userRepository.GetByIdAsync(request.UserId)
                ?? throw new ArgumentException("No se encontró el usuario seleccionado.");
            List<Zone> zones = new List<Zone>();
            foreach (var zoneId in request.ZoneIds)
            {
                Zone zone = await _zoneRepository.GetByIdAsync(zoneId)
                    ?? throw new ArgumentException($"No se encontró la zona con ID {zoneId}.");
                zones.Add(zone);
            }

            List<Delivery>  deliveriesFromZones = new List<Delivery>();
            List<Delivery> deliveriesFromClients = new List<Delivery>();

            deliveriesFromZones = await _deliveryRepository.GetDeliveriesPendingByZoneIdsAsync(request.ZoneIds);
            deliveriesFromClients = await _deliveryRepository.GetDeliveriesPendingByClientIdsAsync(request.ClientIds);

            List<Delivery> deliveriesAll = deliveriesFromZones.Concat(deliveriesFromClients).GroupBy(d => d.Id) .Select(g => g.First()) 
    .ToList();
            List<DistributionDelivery> distributionDeliveries = new List<DistributionDelivery>();
            int i = 1;
            foreach (var delivery in deliveriesAll)
            {
                distributionDeliveries.Add(new DistributionDelivery(delivery, i));
            }

            Distribution newDistribution = new Distribution(request.Observations, user, vehicle, zones, distributionDeliveries);
            newDistribution.AuditInfo.SetCreated(request.getUserId(), request.AuditLocation);
            newDistribution.Validate();

            var added = await _distributionRepository.AddAsync(newDistribution);
            return DistributionMapper.ToResponse(added);
        }

        public async Task<DistributionResponse> UpdateDistributionAsync(DistributionUpdateRequest request)
        {
            var existing = await _distributionRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la distribución seleccionada.");

            var user = request.UserId.HasValue
                ? await _userRepository.GetByIdAsync(request.UserId.Value)
                    ?? throw new ArgumentException("No se encontró el usuario seleccionado.")
                : existing.User;

            var vehicle = request.VehicleId.HasValue
                ? await _vehicleRepository.GetByIdAsync(request.VehicleId.Value)
                    ?? throw new ArgumentException("No se encontró el vehículo seleccionado.")
                : existing.Vehicle;

            List<Delivery> deliveries;
            if (request.deliveryIds is null)
            {
                deliveries = await _deliveryRepository.GetDeliveriesByDistributionIdAsync(existing.Id);
            }
            else
            {
                deliveries = new List<Delivery>(request.deliveryIds.Count);
                foreach (var delId in request.deliveryIds)
                {
                    var delivery = await _deliveryRepository.GetByIdAsync(delId)
                        ?? throw new ArgumentException($"No se encontró la entrega con ID {delId}.");
                    deliveries.Add(delivery);
                }
            }

            var distributionDeliveries = deliveries
                .Select((d, idx) => new DistributionDelivery(d, idx + 1))
                .ToList();

            var updatedData = DistributionMapper.ToUpdatableData(
                request,
                user,
                vehicle,
                distributionDeliveries
            );

            existing.Update(updatedData);
            existing.Validate();

            var updated = await _distributionRepository.UpdateAsync(existing);
            return DistributionMapper.ToResponse(updated);
        }


        public async Task<DistributionResponse> DeleteDistributionAsync(DeleteRequest request)
        {
            var distribution = await _distributionRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró la distribución seleccionada.");

            distribution.MarkAsDeleted(request.getUserId(), request.AuditLocation);
            var deleted = await _distributionRepository.DeleteAsync(distribution);

            return DistributionMapper.ToResponse(deleted);
        }



        public async Task<DistributionResponse> GetDistributionByIdAsync(int id)
        {
            var distribution = await _distributionRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la marca seleccionada.");

            List<Delivery> deliveries = new List<Delivery>();
            deliveries = await _deliveryRepository.GetDeliveriesByDistributionIdAsync(id);

            distribution.DistributionDeliveries = deliveries.Select((delivery, index) => new DistributionDelivery(delivery, index + 1)).ToList();
            return DistributionMapper.ToResponse(distribution);
        }

        public async Task<List<DistributionResponse>> GetAllDistributionsAsync(QueryOptions options)
        {
            var distributions = await _distributionRepository.GetAllAsync(options);
            foreach (var distribution in distributions)
            {
                List<Delivery> deliveries = await _deliveryRepository.GetDeliveriesByDistributionIdAsync(distribution.Id);
                distribution.DistributionDeliveries = deliveries.Select((delivery, index) => new DistributionDelivery(delivery, index + 1)).ToList();
            }
            return distributions.Select(DistributionMapper.ToResponse).ToList();
        }
    }
}
