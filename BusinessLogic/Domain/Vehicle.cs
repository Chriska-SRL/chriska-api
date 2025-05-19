namespace BusinessLogic.Dominio
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int CrateCapacity { get; set; }
        public Delivery Delivery { get; set; }


    }
}
