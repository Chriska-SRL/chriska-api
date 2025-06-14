﻿namespace BusinessLogic.Dominio
{
    public class StockMovement:IEntity<StockMovement.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public Shelve Shelve { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }

        public StockMovement(int id,DateTime date, int quantity, string type, string reason, Shelve shelve, User user,Product product)
        {
            Id = id;
            Date = date;
            Quantity = quantity;
            Type = type;
            Reason = reason;
            Shelve = shelve;
            User = user;
            Product = product;
        }

        public void Validate()
        {

            if (Date == null) throw new Exception("La fecha no puede estar vacía");
            if (Quantity <= 0) throw new Exception("La cantidad debe ser mayor a cero");
            if (string.IsNullOrEmpty(Type)) throw new Exception("El tipo de movimiento no puede estar vacío");
            if (string.IsNullOrEmpty(Reason)) throw new Exception("El motivo del movimiento no puede estar vacío");
            if (Shelve == null) throw new Exception("El estante no puede estar vacío");
            if (User == null) throw new Exception("El usuario no puede estar vacío");
            if (Product == null) throw new Exception("El producto no puede estar vacío");
        }
        public void Update(UpdatableData data)
        {
            Date = data.Date;
            Quantity = data.Quantity;
            Type = data.Type;
            Reason = data.Reason;
            Shelve = data.Shelve;
            User = data.User;
            Product = data.Product;
            Validate();
        }
        public class UpdatableData
        {
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
            public string Type { get; set; }
            public string Reason { get; set; }
            public Shelve Shelve { get; set; }
            public User User { get; set; }
            public Product Product { get; set; }
        }

    }
}
