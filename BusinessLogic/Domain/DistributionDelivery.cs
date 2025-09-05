namespace BusinessLogic.Domain
{
    public class DistributionDelivery
    {
        public Delivery Delivery { get; set; }
        public int Position { get; set; }

        public DistributionDelivery(Delivery delivery, int position)
        {
            Delivery = delivery ?? throw new ArgumentException(nameof(delivery));
            this.Position = position;
        }
    }
}