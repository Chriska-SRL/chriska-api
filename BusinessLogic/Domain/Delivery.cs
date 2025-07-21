using BusinessLogic.Común;

namespace BusinessLogic.Dominio
{
    public class Delivery:IEntity<Delivery.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DriverName { get; set; }
        public string Observation { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public Vehicle Vehicle { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Delivery(int id, DateTime date, string driverName, string observation, List<Order> orders, Vehicle vehicle)
        {
            Id = id;
            Date = date;
            DriverName = driverName;
            Observation = observation;
            Orders = orders;
            Vehicle = vehicle;
        }
        public Delivery(int id)
        {
            Id = id;
          //Date = date;
          // DriverName = driverName;
          //  Observation = observation;
          //  Orders = orders;
           // Vehicle = vehicle;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(DriverName)) throw new Exception("El nombre del conductor no puede estar vacío");
            if (string.IsNullOrEmpty(Observation)) throw new Exception("La observación no puede estar vacía");
            if (Vehicle == null) throw new Exception("El vehículo no puede estar vacío");
        }

        public void Update(UpdatableData Data)
        {
            Date = Data.Date;
            DriverName = Data.DriverName;
            Observation = Data.Observation;
            Vehicle = Data.Vehicle;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public string DriverName { get; set; }
            public string Observation { get; set; }
            public Vehicle Vehicle { get; set; }
            public AuditInfo AuditInfo { get; set; }
        }
    }
}
