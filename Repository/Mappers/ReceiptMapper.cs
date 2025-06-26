using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ReceiptMapper
    {
        public static Receipt FromReader(SqlDataReader reader)
        {
            var client = new Client(
                id: reader.GetInt32(reader.GetOrdinal("ClientId")),
                name: reader.GetString(reader.GetOrdinal("ClientName")),
                rut: reader.GetString(reader.GetOrdinal("ClientRUT")),
                razonSocial: reader.GetString(reader.GetOrdinal("ClientRazonSocial")),
                address: reader.GetString(reader.GetOrdinal("ClientAddress")),
                mapsAddress: reader.GetString(reader.GetOrdinal("ClientMapsAddress")),
                schedule: reader.GetString(reader.GetOrdinal("ClientSchedule")),
                phone: reader.GetString(reader.GetOrdinal("ClientPhone")),
                contactName: reader.GetString(reader.GetOrdinal("ClientContactName")),
                email: reader.GetString(reader.GetOrdinal("ClientEmail")),
                observations: reader.GetString(reader.GetOrdinal("ClientObservations")),
                bank: reader.GetString(reader.GetOrdinal("ClientBank")),
                bankAccount: reader.GetString(reader.GetOrdinal("ClientBankAccount")),
                loanedCrates: reader.GetInt32(reader.GetOrdinal("ClientLoanedCrates")),
                zone: new Zone(
                    id: reader.GetInt32(reader.GetOrdinal("ZoneId")),
                    name: reader.GetString(reader.GetOrdinal("ZoneName")),
                    description: reader.GetString(reader.GetOrdinal("ZoneDescription"))
                )
            );

            return new Receipt(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                date: reader.GetDateTime(reader.GetOrdinal("Date")),
                amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                notes: reader.GetString(reader.GetOrdinal("Notes")),
                client: client,
                paymentMethod: reader.GetString(reader.GetOrdinal("PaymentMethod"))
            );
        }
    }
}
    

