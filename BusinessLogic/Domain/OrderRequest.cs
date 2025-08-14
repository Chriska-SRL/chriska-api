using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public class OrderRequest : Request, IEntity<OrderRequest.UpdatableData>
    {
        public Order Order { get; set; }

        public OrderRequest(
          Client? client,
          Status status,
          DateTime? date,
          string? observation,
          User? user,
          List<ProductItem> productItems,
          AuditInfo? auditInfo,
          Order order
      ) : base(client, status, date, observation, user, productItems, auditInfo)
        {
            Order = order;
        }

        public OrderRequest(
            int id,
            Client? client,
            Status status,
            DateTime? date,
            string? observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo,
            Order order
        ) : base(id, client, status, date, observation, user, productItems, auditInfo)
        {
            Order = order;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
        public void Update(UpdatableData updatableData)
        {
            
        }
        public class UpdatableData
        {
           
        }
    }
}
