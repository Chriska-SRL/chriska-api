namespace BusinessLogic.Dominio
{
    public class Vehicle : IEntity<Vehicle.UpdatableData>
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public Cost Cost { get; set; }

        public Vehicle(int id, string plate, string brand, string model, int crateCapacity, Cost cost)
        {
            Id = id;
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
            Cost = cost;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Plate))
                throw new ArgumentException("Plate cannot be empty or null.");
            if (string.IsNullOrWhiteSpace(Brand))
                throw new ArgumentException("Brand cannot be empty or null.");
            if (string.IsNullOrWhiteSpace(Model))
                throw new ArgumentException("Model cannot be empty or null.");
            if (CrateCapacity <= 0)
                throw new ArgumentException("Crate capacity must be greater than zero.");
            if (Cost == null)
                throw new ArgumentException("Cost cannot be null.");
        }

        public void Update(UpdatableData data)
        {
            Plate = data.Plate;
            Brand = data.Brand;
            Model = data.Model;
            CrateCapacity = data.CrateCapacity;
            Cost = data.Cost;
            Validate();
        }
        public class UpdatableData
        {
            public string Plate { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public int CrateCapacity { get; set; }
            public Cost Cost { get; set; }

        }
    }
}
