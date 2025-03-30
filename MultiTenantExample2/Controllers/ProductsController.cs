using Microsoft.AspNetCore.Mvc;
using MultiTenantExample2.Dtos;

namespace MultiTenantExample2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var allProducts = await _productService.GetAllAsync();
            return Ok(allProducts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateProductDto createProductDto)
        {
            Product product = new()
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Rate = createProductDto.Rate
            };

            var createdProduct = await _productService.CreateAsync(product);

            return Ok(createdProduct);
        }
    }
}
