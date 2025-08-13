using BusinessLogic.Common.Enums;
using BusinessLogic.Common;

namespace BusinessLogic.Domain
{
    public abstract class Request:ClientDocument
    {
        public Request(
            Client? client,
            Status status,
            DateTime? date,
            DateTime confirmedDate,
            string? observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo
        ) : base(client, status, confirmedDate, date ,observation, user, productItems, auditInfo)
        {
        }

        public Request(
            int id,
            Client? client,
            Status status,
            DateTime? date,
            DateTime confirmedDate,
            string? observation,
            User? user,
            List<ProductItem> productItems,
            AuditInfo? auditInfo
        ) : base(id, client, status, confirmedDate, date, observation, user, productItems, auditInfo)
        {
        }

    }
}
