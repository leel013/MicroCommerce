using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.RabbitMQ;
using SharedContract.Model;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RabbitMQPublisher _publisher;
        private static readonly List<Order> Orders = new();

        public OrdersController(IHttpClientFactory httpClientFactory, RabbitMQPublisher publisher)
        {
            _httpClientFactory = httpClientFactory;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(
    $"http://productservice:8080/api/products/{request.ProductId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest("Product Not Found");

            var json = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<ProductResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (product!.Stock < request.Quantity)
                return BadRequest("Stock not available");

            var order = new Order
            {
                OrderId = Orders.Count + 1,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            Orders.Add(order);
            await _publisher.PublishOrderCreated(new OrderCreatedEvent
            {
                OrderId = order.OrderId,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            });

            return Ok(order);
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(Orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = Orders.FirstOrDefault(x => x.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
