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
            Validate();
        }

        public void Validate()
        {
            if (Date == default)
                throw new ArgumentNullException(nameof(Date), "La fecha es obligatoria.");

            if (Amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(Amount), "El monto debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(PaymentMethod))
                throw new ArgumentNullException(nameof(PaymentMethod), "El método de pago es obligatorio.");
            if (PaymentMethod.Length > 30)
                throw new ArgumentOutOfRangeException(nameof(PaymentMethod), "El método de pago no puede superar los 30 caracteres.");

            if (!string.IsNullOrWhiteSpace(Notes) && Notes.Length > 250)
                throw new ArgumentOutOfRangeException(nameof(Notes), "Las notas no pueden superar los 250 caracteres.");

        }
        public void Update(UpdatableData updatableData)
        {
            Date = updatableData.Date;
            Amount = updatableData.Amount;
            PaymentMethod = updatableData.PaymentMethod;
            Notes = updatableData.Notes;
            Client = updatableData.Client;
            Validate();
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
