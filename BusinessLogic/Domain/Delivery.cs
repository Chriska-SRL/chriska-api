namespace BusinessLogic.Dominio
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DriverName { get; set; }
        public string Observation { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public Vehicle Vehicle { get; set; }

        public Delivery(DateTime date, string driverName, string observation, Vehicle vehicle)
        {
            Date = date;
            DriverName = driverName;
            Observation = observation;
            Vehicle = vehicle;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(DriverName)) throw new Exception("El nombre del conductor no puede estar vacío");
            if (string.IsNullOrEmpty(Observation)) throw new Exception("La observación no puede estar vacía");
            if (Vehicle == null) throw new Exception("El vehículo no puede estar vacío");
        }

        public void Update(string driverName, string observation, Vehicle vehicle)
        {
            DriverName = driverName;
            Observation = observation;
            Vehicle = vehicle;
        }
    }
}
