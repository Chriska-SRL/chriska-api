using System.Net.Http.Headers;

namespace BusinessLogic.Dominio
{
    public class Cost:IEntity<Cost.UpdatableData>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public Cost(int id, string description, decimal amount)
        {
            Id = id;
            Description = description;
            Amount = amount;
        }


        public void Validate()
        {
            if(string.IsNullOrWhiteSpace(Description)) throw new Exception("Description cannot be null or empty.");
            if (Amount <= 0) throw new Exception("Amount must be greater than zero.");
        }
        public void Update (UpdatableData data)
        {
            Description = data.Description;
            Amount = data.Amount;
        }
        public class UpdatableData
        {
            public string Description { get; set; }
            public decimal Amount { get; set; }
        }

    }
}
