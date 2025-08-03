using BusinessLogic.Común;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
namespace Repository.Mappers
{
    public static class ZoneMapper
    {
        public static Zone FromReader(SqlDataReader reader)
        {
            var zone = new Zone(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                image: reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString(reader.GetOrdinal("ImageUrl")),
                deliveryDays: new List<Day>(),
                requestDays: new List<Day>(),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );

 
            if (!reader.IsDBNull(reader.GetOrdinal("DeliveryDays")))
            {
                var deliveryDaysStr = reader.GetString(reader.GetOrdinal("DeliveryDays"));
                zone.DeliveryDays = deliveryDaysStr
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim()))
                    .ToList();
            }

            if (!reader.IsDBNull(reader.GetOrdinal("RequestDays")))
            {
                var requestDaysStr = reader.GetString(reader.GetOrdinal("RequestDays"));
                zone.RequestDays = requestDaysStr
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim()))
                    .ToList();
            }

            return zone;
        }
    }
}
