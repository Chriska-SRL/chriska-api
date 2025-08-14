using BusinessLogic.Domain;


namespace BusinessLogic.Domain
{
    public class ProductDocument
    {
        public int Id { get; set; }
        public Purchase? Purchase { get; set; }

    }
}
