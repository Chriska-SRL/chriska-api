using BusinessLogic.Common;
using BusinessLogic.Common.Enums;

namespace BusinessLogic.Domain
{
    public class Order : ClientDocument, IEntity<Order.UpdatableData>
    {
        public int Crates { get; set; }
        public OrderRequest? OrderRequest { get; set; }
        public Delivery? Delivery { get; set; }
        public Order(
           Client? client,
           Status status,
           DateTime? date,
           DateTime confirmedDate,
           string? observation,
           User? user,
           List<ProductItem> productItems,
           AuditInfo? auditInfo,
           int crates,
           OrderRequest? orderRequest = null,
           Delivery? delivery = null
       ) : base(client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Crates = crates;
            OrderRequest = orderRequest;
            Delivery = delivery;
        }
        public Order(
          int id,
          Client? client,
          Status status,
          DateTime? date,
          string? observation,
          User? user,
          List<ProductItem> productItems,
          AuditInfo? auditInfo,
          DateTime confirmedDate,
          int crates,
          OrderRequest? orderRequest = null,
          Delivery? delivery = null
      ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
            Crates = crates;
            OrderRequest = orderRequest;
            Delivery = delivery;
        }



        public void Update(UpdatableData data)
        {
            Crates = data.Crates;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public class UpdatableData
        {
            public int Crates { get; set; }
        }
    }
}
