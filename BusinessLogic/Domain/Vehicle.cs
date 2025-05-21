namespace BusinessLogic.Dominio
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public Cost cost { get; set; }

        public Vehicle(string plate, string brand, string model, int crateCapacity)
        {
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
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
        }
        public void Update(string plate, string brand, string model, int crateCapacity)
        {
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
            
        }  


    }
}
