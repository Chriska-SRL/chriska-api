namespace BusinessLogic.Dominio
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public Cost Cost { get; set; }

        public Vehicle(string plate, string brand, string model, int crateCapacity,Cost cost)
        {
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
            Cost = cost;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Plate))
                throw new Exception("La placa no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(Brand))
                throw new Exception("La marca no puede estar vacía.");
            if (string.IsNullOrWhiteSpace(Model))
                throw new Exception("El modelo no puede estar vacío.");
            if (CrateCapacity <= 0)
                throw new Exception("La capacidad de cajas debe ser mayor que cero.");
            if (Cost == null)
                throw new Exception("El costo no puede ser nulo.");
        }

        public void Update(string plate, string brand, string model, int crateCapacity,Cost cost)
        {
            Plate = plate;
            Brand = brand;
            Model = model;
            CrateCapacity = crateCapacity;
            Cost = cost;
        } 
    }
}
