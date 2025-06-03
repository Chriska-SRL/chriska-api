namespace BusinessLogic.Dominio
{
    public class CreditNote : IEntity<CreditNote.UpdatableData>
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public ReturnRequest ReturnRequest { get; set; }

        public CreditNote(int id, DateTime issueDate, ReturnRequest returnRequest)
        {
            Id = id;
            IssueDate = issueDate;
            ReturnRequest = returnRequest;
        }
        public void Validate()
        {
            if (ReturnRequest == null) throw new Exception("La solicitud de devolucion es obligatoria para una nota de credito");
            if (IssueDate == null) throw new Exception("La fecha de emision es obligatoria para una nota de credito");
        }
        public void Update(UpdatableData data)
        {
            IssueDate = data.IssueDate;
            ReturnRequest = data.ReturnRequest;
            Validate();
        } 
        public class UpdatableData
        {
            public DateTime IssueDate { get; set; }
            public ReturnRequest ReturnRequest { get; set; }
        }
    }
}
