using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public abstract class Request : ClientDocument
    {
        public Request(
            Client client,
            string? observation,
            User user,
            List<ProductItem> productItems
        ) : base(client, observation, user, productItems)
        {
        }

        public Request(
            int id,
            Client? client,
            Status status,
           DateTime? confirmedDate,
           DateTime date,
            string observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo
        ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
        }

    }
}