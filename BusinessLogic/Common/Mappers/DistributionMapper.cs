using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsDistribution;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Common.Mappers
{
    public static class DistributionMapper
    {
        public static Distribution ToDomain(
            DistributionAddRequest request,
            User user,
            Vehicle vehicle,
            List<Zone> zones,
            List<DistributionDelivery> distributionDeliveries)
        {
            var dist = new Distribution(
                observations: request.Observations ?? string.Empty,
                user: user,
                vehicle: vehicle,
                zones: zones ?? new List<Zone>(),
                distributionDeliveries: distributionDeliveries ?? new List<DistributionDelivery>()
            );

            dist.AuditInfo?.SetCreated(request.getUserId(), request.Location);
            return dist;
        }

        public static DistributionResponse ToResponse(Distribution distribution)
        {
            return new DistributionResponse
            {
                Id = distribution.Id,
                Observations = distribution.Observations,
                User = UserMapper.ToResponse(distribution.User),
                Vehicle = VehicleMapper.ToResponse(distribution.Vehicle),
                Zones = distribution.Zones.Select(z => ZoneMapper.ToResponse(z)).ToList(),
                Deliveries = distribution.distributionDeliveries
                    .Select(dd => DeliveryMapper.ToResponse(dd.Delivery))
                    .ToList()
            };
        }

        public static Distribution.UpdatableData ToUpdatableData(
            DistributionUpdateRequest request,
            User? user,
            Vehicle? vehicle,
            List<DistributionDelivery>? distributionDeliveries)
        {
            return new Distribution.UpdatableData
            {
                Observations = request.Observations,
                User = user,
                Vehicle = vehicle,
                DistributionDeliveries = distributionDeliveries,

                UserId = request.getUserId(),
                Location = request.Location
            };
        }
    }
}
