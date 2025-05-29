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
            if (Date == default)
                throw new ArgumentException("Fecha no puede ser nula o por defecto.");
            if (Amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");
            if (string.IsNullOrWhiteSpace(PaymentMethod))
                throw new ArgumentException("El metodo de pago no puede estar vacio.");
            if (Client == null)
                throw new ArgumentException("El cliente no puede ser nulo.");
        }

        public void Update(decimal amount, string paymentMethod, string notes, Client client)
        {          
            Amount = amount;
            PaymentMethod = paymentMethod;
            Notes = notes;
            Client = client;
        }
    }
}
