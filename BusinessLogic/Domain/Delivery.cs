
using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class Delivery : ClientDocument
    {
        public int Crates { get; set; }
        public Order? Order { get; set; }
        public Delivery(
                  string observation,
                  User user,
                  int crates,
                  Order order
              ) : base(order.Client, observation, user, order.ProductItems)
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

    }
}