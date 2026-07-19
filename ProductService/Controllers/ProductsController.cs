using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products =
   [
       new Product
        {
            Id = 1,
            Name = "Nike Shoes",
            Price = 5000,
            Stock = 10
        },
        new Product
        {
            Id = 2,
            Name = "Adidas Shoes",
            Price = 7000,
            Stock = 5
        }
   ];

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound("Product Not Found");

            return Ok(product);
        }
    }
}
