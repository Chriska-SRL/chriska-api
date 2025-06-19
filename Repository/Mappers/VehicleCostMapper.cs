using BusinessLogic.Dominio;
using BusinessLogic.Común.Enums;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class VehicleCostMapper
    {
        public static VehicleCost FromReader(SqlDataReader reader)
        {
            return new VehicleCost(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                vehicleId: reader.GetInt32(reader.GetOrdinal("VehicleId")),
                type: (VehicleCostType)reader.GetInt32(reader.GetOrdinal("Type")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                date: reader.GetDateTime(reader.GetOrdinal("Date"))
            );
        }
    }

}
