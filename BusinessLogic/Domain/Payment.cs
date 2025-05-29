namespace BusinessLogic.Dominio
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public Supplier Supplier { get; set; }

        public Payment( DateTime date, decimal amount, string paymentMethod, string note, Supplier supplier)
        {
            Date = date;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Note = note;
            Supplier = supplier;
        }

        public void Validate()
        {
            if (Date == null) throw new Exception("La fecha no puede estar vacía");
            if (Amount <= 0) throw new Exception("El monto debe ser mayor a cero");
            if (string.IsNullOrEmpty(PaymentMethod)) throw new Exception("El metodo de pago no puede estar vacío");
            if (Supplier == null) throw new Exception("El proveedor no puede estar vacío");
        }

        public void Update(DateTime date, decimal amount, string paymentMethod, string note, Supplier supplier)
        {
            Date = date;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Note = note;
            Supplier = supplier;
        }
    }
}
