using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;


namespace Repository.Mappers
{
    public static class ClientMapper
    {
        public static Client FromReader(SqlDataReader reader)
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
                _ => Bank.Otros // Por si viene algo no esperado
            };

            return new Client(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                rut: reader.GetString(reader.GetOrdinal("RUT")),
                razonSocial: reader.GetString(reader.GetOrdinal("RazonSocial")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                mapsAddress: reader.GetString(reader.GetOrdinal("MapsAddress")),
                schedule: reader.GetString(reader.GetOrdinal("Schedule")),
                phone: reader.GetString(reader.GetOrdinal("Phone")),
                contactName: reader.GetString(reader.GetOrdinal("ContactName")),
                email: reader.GetString(reader.GetOrdinal("Email")),
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                bank: bank,
                bankAccount: reader.GetString(reader.GetOrdinal("BankAccount")),
                loanedCrates: reader.GetInt32(reader.GetOrdinal("LoanedCrates")),
                zone: new Zone(
                    id: reader.GetInt32(reader.GetOrdinal("ZoneId")),
                    name: reader.GetString(reader.GetOrdinal("ZoneName")),
                    description: reader.GetString(reader.GetOrdinal("ZoneDescription"))
                )
            );
        }


    }
}
