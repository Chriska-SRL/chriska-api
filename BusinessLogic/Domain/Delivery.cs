
using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Delivery : ClientDocument, IEntity<Delivery.UpdatableData>
    {
        public int Crates { get; set; }
        public Order? Order{ get; set; }
        public Delivery(
                  Client? client,
                  Status status,
                  DateTime confirmedDate,
                  DateTime? date,
                  string? observation,
                  User? user,
                  List<ProductItem> productItems,
                  AuditInfo? auditInfo,
                  int crates,
                  Order? order 
              ) : base(client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Crates = crates;
            Order = order;
        }

        public Delivery(
            int id,
            Client client,
            Status status,
            DateTime confirmedDate,
            DateTime date,
            string observation,
            User user,
            List<ProductItem> productItems,
            AuditInfo auditInfo,
            int crates,
            Order order
        ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Crates = crates;
            Order = order;
        }
        public override void Validate()
        {
            throw new NotImplementedException();
        }
        public void Update(UpdatableData data)
        {
            ConfirmedDate = data.ConfirmedDate;
            Crates = data.Crates;
        }
        public class UpdatableData
        {
            public DateTime ConfirmedDate { get; set; }
            public int Crates { get; set; }
        }
    }
}
