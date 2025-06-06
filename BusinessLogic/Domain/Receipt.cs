namespace BusinessLogic.Dominio
{
    public class Receipt : IEntity<Receipt.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public Client Client { get; set; }

        public Receipt(int id,DateTime date, decimal amount, string paymentMethod, string notes, Client client)
        {
            Id = id;
            Date = date;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Notes = notes;
            Client = client;
        }

        public void Validate()
        {
            if (Date == null) throw new Exception ("La fecha no puede estar vacia.");
            if (Amount <= 0) throw new Exception("El monto debe ser mayor a cero.");
            if (string.IsNullOrEmpty(PaymentMethod)) throw new Exception("El método de pago no puede estar vacío.");
            if (Client == null) throw new Exception("El cliente no puede estar vacío.");
        }
        public void Update(UpdatableData updatableData)
        {
            Date = updatableData.Date;
            Amount = updatableData.Amount;
            PaymentMethod = updatableData.PaymentMethod;
            Notes = updatableData.Notes;
            Client = updatableData.Client;
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string PaymentMethod { get; set; }
            public string Notes { get; set; }
            public Client Client{ get; set; }
        }
    }
}
