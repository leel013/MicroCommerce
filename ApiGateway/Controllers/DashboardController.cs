using ApiGateway.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // OrderService call 
            var orderResponse =
                await client.GetAsync($"http://orderservice:8080/api/orders/{id}");

            if (!orderResponse.IsSuccessStatusCode)
                return NotFound("Order Not Found");

            var orderJson = await orderResponse.Content.ReadAsStringAsync();

            var order = JsonSerializer.Deserialize<Order>(
                orderJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // ProductService call 
            var productResponse =
                await client.GetAsync($"http://productservice:8080/api/products/{order!.ProductId}");

            var productJson = await productResponse.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<Product>(
                productJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var result = new OrderDetailsResponse
            {
                OrderId = order.OrderId,
                Quantity = order.Quantity,
                Product = product
            };

            return Ok(result);
        }
    }
}
