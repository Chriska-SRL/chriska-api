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

            List<Delivery> deliveriesAll = deliveriesFromZones.Concat(deliveriesFromClients).ToList();
            List<DistributionDelivery> distributionDeliveries = new List<DistributionDelivery>();
            int i = 1;
            foreach (var delivery in deliveriesAll)
            {
                distributionDeliveries.Add(new DistributionDelivery(delivery, i));
            }

            Distribution newDistribution = new Distribution(request.Observations, user, vehicle, zones, distributionDeliveries);
            newDistribution.AuditInfo.SetCreated(request.getUserId(), request.Location);
            newDistribution.Validate();

            var added = await _distributionRepository.AddAsync(newDistribution);
            return DistributionMapper.ToResponse(added);
        }

        public async Task<DistributionResponse> UpdateDistributionAsync(DistributionUpdateRequest request)
        {
            throw new NotImplementedException("El método UpdateDistributionAsync aún no está implementado.");
        }

        public async Task<DistributionResponse> DeleteDistributionAsync(DeleteRequest request)
        {
            throw new NotImplementedException("El método DeleteDistributionAsync aún no está implementado.");
        }


        public async Task<DistributionResponse> GetDistributionByIdAsync(int id)
        {
            var distribution = await _distributionRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró la marca seleccionada.");

            List<Delivery> deliveries = new List<Delivery>();
            deliveries = await _deliveryRepository.GetDeliveriesByDistributionIdAsync(id);

            distribution.distributionDeliveries = deliveries.Select((delivery, index) => new DistributionDelivery(delivery, index + 1)).ToList();
            return DistributionMapper.ToResponse(distribution);
        }

        public async Task<List<DistributionResponse>> GetAllDistributionsAsync(QueryOptions options)
        {
            var distributions = await _distributionRepository.GetAllAsync(options);
            foreach (var distribution in distributions)
            {
                List<Delivery> deliveries = await _deliveryRepository.GetDeliveriesByDistributionIdAsync(distribution.Id);
                distribution.distributionDeliveries = deliveries.Select((delivery, index) => new DistributionDelivery(delivery, index + 1)).ToList();
            }
            return distributions.Select(DistributionMapper.ToResponse).ToList();
        }
    }
}
