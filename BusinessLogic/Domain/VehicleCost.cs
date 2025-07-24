using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
namespace BusinessLogic.Dominio
{
    public class VehicleCost:IEntity<VehicleCost.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
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

        public VehicleCost(int id, int vehicleId)
        {
            // Constructor temporal utilizado únicamente para instanciar por Id.
            // No debe usarse para lógica de negocio que requiera datos válidos.
            Id = id;
            VehicleId = vehicleId;
            Type = VehicleCostType.Other;
            Description = "Descripción Temporal";
            Amount = 0;
            Date = DateTime.Today;
            // No se llama a Validate() porque no se pretende usar la instancia completa
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
            AuditInfo = data.AuditInfo ?? AuditInfo;

            Validate();
        }

        public void SetDeletedAudit(AuditInfo auditInfo)
        {
            if (auditInfo == null)
                throw new ArgumentNullException(nameof(auditInfo), "La información de auditoría no puede ser nula.");
            AuditInfo.DeletedAt = auditInfo.DeletedAt;
            AuditInfo.DeletedBy = auditInfo.DeletedBy;
            AuditInfo.DeletedLocation = auditInfo.DeletedLocation;
        }

        public class UpdatableData
        {
            public VehicleCostType? Type { get; set; }
            public string? Description { get; set; }
            public decimal? Amount { get; set; }
            public DateTime? Date { get; set; }
            public AuditInfo? AuditInfo { get; set; } = null;
        }
    }
}
