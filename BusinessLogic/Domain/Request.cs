namespace BusinessLogic.Dominio
{
    public abstract class Request
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
        public User User { get; set; }
        public Client Client { get; set; }

        public abstract void Validate();

   
}
}
