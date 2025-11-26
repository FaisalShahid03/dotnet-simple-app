using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day5MiniProject.Data;
using Day5MiniProject.Models;

namespace Day5MiniProject.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public ProductsController(AppDbContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll() => await _ctx.Products.Include(p => p.Category).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var p = await _ctx.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            return p;
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Product>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await _ctx.Products.Include(p => p.Category).ToListAsync();
            var q = name.Trim().ToLowerInvariant();
            return await _ctx.Products.Include(p => p.Category)
                                       .Where(p => p.Name.ToLower().Contains(q))
                                       .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            // Validate category exists
            var cat = await _ctx.Categories.FindAsync(product.CategoryId);
            if (cat == null) return BadRequest($"Category {product.CategoryId} not found.");

            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            if (id != updated.Id) return BadRequest();
            var existing = await _ctx.Products.FindAsync(id);
            if (existing == null) return NotFound();

            // validate category
            var cat = await _ctx.Categories.FindAsync(updated.CategoryId);
            if (cat == null) return BadRequest($"Category {updated.CategoryId} not found.");

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Price = updated.Price;
            existing.CategoryId = updated.CategoryId;

            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _ctx.Products.FindAsync(id);
            if (p == null) return NotFound();
            _ctx.Products.Remove(p);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}