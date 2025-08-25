using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{origin ?? ""}{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            // Si viene con prefijo pero no hay fila/columna válida, no mapeamos
            if (prefix != null)
            {
                try
                {
                    int o = r.GetOrdinal(Col("Id"));
                    if (r.IsDBNull(o)) return null;
                }
                catch
                {
                    return null;
                }
            }

            return new Vehicle(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                plate: S(Col("Plate")),
                brand: S(Col("Brand")),
                model: S(Col("Model")),
                crateCapacity: r.GetInt32(r.GetOrdinal(Col("CrateCapacity"))),
                costs: new List<VehicleCost>(),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
