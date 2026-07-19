namespace ApiGateway.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        public int Stock { get; set; }
    }

    public class OrderDetailsResponse
    {
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public Product? Product { get; set; }
    }
}
