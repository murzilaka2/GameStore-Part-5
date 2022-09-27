namespace GameStore.Models
{
    public class Cart
    {
        private List<OrderLine> selections = new List<OrderLine>();

        public IEnumerable<OrderLine> Selections { get => selections; }

        public Cart AddItem(Product p, int quantity)
        {
            OrderLine orderLine = selections.Where(e => e.ProductId == p.Id).FirstOrDefault();
            if (orderLine != null)
            {
                orderLine.Quantity += quantity;
            }
            else
            {
                selections.Add(new OrderLine
                {
                    ProductId = p.Id,
                    Product = p,
                    Quantity = quantity
                });
            }
            return this;
        }
        public Cart RemoveItem(int productId)
        {
            selections.RemoveAll(e => e.ProductId == productId);
            return this;
        }
        public void Clear() => selections.Clear();
    }
}
