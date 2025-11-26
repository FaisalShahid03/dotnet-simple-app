using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day5MiniProject.Data;
using Day5MiniProject.Models;

namespace Day5MiniProject.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public CategoriesController(AppDbContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAll() => await _ctx.Categories.ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var c = await _ctx.Categories.FindAsync(id);
            if (c == null) return NotFound();
            return c;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Category updated)
        {
            if (id != updated.Id) return BadRequest();
            var cat = await _ctx.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            cat.Name = updated.Name;
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _ctx.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            _ctx.Categories.Remove(cat);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}