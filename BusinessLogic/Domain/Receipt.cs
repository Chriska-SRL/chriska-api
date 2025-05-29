namespace BusinessLogic.Dominio
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public Client Client { get; set; }

        public Receipt(DateTime date, decimal amount, string paymentMethod, string notes, Client client)
        {
            Date = date;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Notes = notes;
            Client = client;
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
