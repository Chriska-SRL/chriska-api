namespace BusinessLogic.DTOs.DTOsVehicle
{
    public class AddVehicleRequest
    {
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
    }
}
