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
        public List<DistributionDelivery> DistributionDeliveries { get; set; } = new List<DistributionDelivery>();
        public AuditInfo? AuditInfo { get; set; }

        public decimal getTotal()
        {
            return DistributionDeliveries.Sum(dd => dd.Delivery.getAmount());
        }
        public decimal getPayments()
        {
            return DistributionDeliveries.Sum(dd => dd.Delivery.Payment);
        }
        public int getCrates()
        {
            return DistributionDeliveries.Sum(dd => dd.Delivery.Order.Crates);
        }
        public int getReturnCrates()
        {
            return DistributionDeliveries.Sum(dd => dd.Delivery.Crates);
        }


        public Distribution(string observations, User user, Vehicle? vehicle, List<Zone> zones, List<DistributionDelivery> distributionDeliveries)
        {
            Observations = observations;
            Date = DateTime.Now;
            User = user;
            Vehicle = vehicle;
            Zones = zones ?? new List<Zone>();
            this.DistributionDeliveries = distributionDeliveries ?? new List<DistributionDelivery>();
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
            this.DistributionDeliveries = distributionDeliveries ?? new List<DistributionDelivery>();
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
            DistributionDeliveries = data.DistributionDeliveries ?? DistributionDeliveries;

            AuditInfo.SetUpdated(data.UserId, data.Location);
            Validate();

        }

        public void Validate()
        {
            if (Observations.Length > 255)
                throw new ArgumentException("La observaciones no puede tener mas de 255 caracteres.");
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
