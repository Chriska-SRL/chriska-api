using BusinessLogic.Common.Enums;
using BusinessLogic.Common;
namespace BusinessLogic.Domain
{
    public class Delivery : ClientDocument
    {
        public int Crates { get; set; }
        public Order? Order { get; set; }
        public Delivery(
                  Order order
              ) : base(order.Client, "", order.User, order.ProductItems)
        {
            Id = order.Id;
            Crates = 0;
            Order = order;
        }
        public Delivery(
            int id,
            Client client,
            Status status,
            DateTime? confirmedDate,
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
            if (Crates < 0)
                throw new ArgumentException("La cantidad de cajones no puede ser negativa.");
       }
       internal void Confirm()
        {
            Status = Status.Confirmed;
            ConfirmedDate = DateTime.Now;
        }
        internal void Cancel()
        {
            Status = Status.Cancelled;
        }
    }
}