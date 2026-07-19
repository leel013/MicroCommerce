namespace OrderService.Model
{
    public class Order
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
    public class OrderRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class ProductResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }
}
