using DotNet9_Project.Models;
using DotNet9_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Parameterized constructor for dependency injection
    public class ProductsController(IProductService _service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = _service.GetById(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            _service.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _service.Update(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPost("filter")]
        public IActionResult FilterByCategories([FromBody] List<string> categories)
        {
            if (categories == null || categories.Count == 0)
                return BadRequest("No categories provided.");

            var matching = _service.GetAll().Where(p => categories.Contains(p.Category)).ToList();

            if (!matching.Any())
                return NotFound("No matching products found.");

            return Ok(matching);
        }
    }
}
