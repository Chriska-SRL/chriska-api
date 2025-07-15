using BusinessLogic.Dominio;


namespace BusinessLogic.Domain
{
    public class ProductDocument
    {
        public int Id { get; set; }
        public Purchase? Purchase { get; set; }

    }
}
