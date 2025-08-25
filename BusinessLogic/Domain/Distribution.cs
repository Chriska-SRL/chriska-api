using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Distribution : IEntity<Distribution.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string Observations { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
        public List<Zone> Zones { get; set; }
        public List<DistributionDelivery> distributionDeliveries { get; set; } = new List<DistributionDelivery>();
        public AuditInfo? AuditInfo { get; set; }

        public Distribution(string observations, User user, Vehicle? vehicle, List<Zone> zones, List<DistributionDelivery> distributionDeliveries)
        {
            Observations = observations;
            Date = DateTime.Now;
            User = user;
            Vehicle = vehicle;
            Zones = zones ?? new List<Zone>();
            this.distributionDeliveries = distributionDeliveries ?? new List<DistributionDelivery>();
            AuditInfo = new AuditInfo();
        }
        public Distribution(int id, string observations, DateTime date, User user, Vehicle vehicle, List<Zone> zones, List<DistributionDelivery> distributionDeliveries, AuditInfo? auditInfo)
        {
            Id = id;
            Observations = observations;
            Date = date;
            User = user;
            Vehicle = vehicle;
            Zones = zones ?? new List<Zone>();
            this.distributionDeliveries = distributionDeliveries ?? new List<DistributionDelivery>();
            AuditInfo = auditInfo;
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            AuditInfo.SetDeleted(userId, location);
        }

        public void Update(UpdatableData data)
        {
            Observations = data.Observations ?? Observations;
            Date = data.Date ?? Date;
            User = data.User ?? User;
            Vehicle = data.Vehicle ?? Vehicle;
            Zones = data.Zones ?? Zones;
            distributionDeliveries = data.DistributionDeliveries ?? distributionDeliveries;

            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();

        }

        public void Validate()
        {
           
        }

        public class UpdatableData : AuditData
        {
            public string? Observations { get; set; } = string.Empty;
            public List<DistributionDelivery>? DistributionDeliveries { get; set; }
            public DateTime? Date { get; set; }
            public User? User { get; set; }
            public Vehicle? Vehicle { get; set; }
            public List<Zone>? Zones { get; set; }
        }
    }
}
