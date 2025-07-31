using BusinessLogic.Common;
using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
namespace BusinessLogic.Domain
{
    public class VehicleCost:IEntity<VehicleCost.UpdatableData>, IAuditable
    {
        public int Id { get; set; }= 0;
        public int VehicleId { get; private set; }
        public VehicleCostType Type { get; private set; } = VehicleCostType.Other; 
        public string Description { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public VehicleCost(int id, int vehicleId, VehicleCostType type, string description, decimal amount, DateTime date,AuditInfo auditInfo)
        {
            Id = id;
            VehicleId = vehicleId;
            Type = type;
            Description = description;
            Amount = amount;
            Date = date;
            AuditInfo = auditInfo;
            Validate();
        }
        public VehicleCost(int vehicleId, VehicleCostType type, string description, decimal amount, DateTime date)
        {
            VehicleId = vehicleId;
            Type = type;
            Description = description;
            Amount = amount;
            Date = date;
            Validate();
        }

        public void Validate()
        {
            if (!Enum.IsDefined(typeof(VehicleCostType), Type))
                throw new ArgumentException("El tipo de costo del vehículo no es válido.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción no puede estar vacía.");

            if (Description.Length < 3)
                throw new ArgumentException("La descripción debe tener al menos 3 caracteres.");

            if (Amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            Type = data.Type ?? Type;
            Description = data.Description ?? Description;
            Amount = data.Amount ?? Amount;
            Date = data.Date ?? Date;
            AuditInfo.SetUpdated(data.UserId, data.Location);

            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }

        public class UpdatableData:AuditData
        {
            public VehicleCostType? Type { get; set; }
            public string? Description { get; set; }
            public decimal? Amount { get; set; }
            public DateTime? Date { get; set; }
        }
    }
}
