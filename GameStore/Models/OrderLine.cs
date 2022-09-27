namespace GameStore.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }

}
