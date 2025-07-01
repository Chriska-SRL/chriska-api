using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier FromReader(SqlDataReader reader)
        {
            string bankStr = reader.GetString(reader.GetOrdinal("Bank")).Trim();
            Bank bank = bankStr switch
            {
                "BROU" => Bank.BROU,
                "BBVA" => Bank.BBVA,
                "Santander" => Bank.Santander,
                "Itaú" => Bank.Itau,
                "Scotiabank" => Bank.Scotiabank,
                "HSBC" => Bank.HSBC,
                "Heritage" => Bank.Heritage,
                "Bandes" => Bank.Bandes,
                "Andbank" => Bank.Andbank,
                _ => Bank.Otros
            };

            return new Supplier(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                razonSocial: reader.GetString(reader.GetOrdinal("RazonSocial")),
                rut: reader.GetString(reader.GetOrdinal("RUT")),
                contactName: reader.GetString(reader.GetOrdinal("ContactName")),
                phone: reader.GetString(reader.GetOrdinal("Phone")),
                email: reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                address: reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                mapsAddress: reader.IsDBNull(reader.GetOrdinal("MapsAddress")) ? null : reader.GetString(reader.GetOrdinal("MapsAddress")),
                bank: bank,
                bankAccount: reader.GetString(reader.GetOrdinal("BankAccount")),
                observations: reader.IsDBNull(reader.GetOrdinal("Observations")) ? null : reader.GetString(reader.GetOrdinal("Observations"))
            );
        }
    }
}
