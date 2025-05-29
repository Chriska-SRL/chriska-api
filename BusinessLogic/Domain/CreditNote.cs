namespace BusinessLogic.Dominio
{
    public class CreditNote
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public ReturnRequest ReturnRequest { get; set; }
    }
}
