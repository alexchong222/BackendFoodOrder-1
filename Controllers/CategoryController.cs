using BackendFoodOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFoodOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly FoodOrderContext _context;

        public CategoryController(FoodOrderContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Category/catnameonly
        [HttpGet("catnameonly")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategoryNames()
        {
            var categoryNames = await _context.Categories
                .Select(c => new { c.Name })
                .ToListAsync();

            return categoryNames;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            return categories;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> GetCategories(int id, Category categories)
        {
            if (id != categories.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(categories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return categories;
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategories(Category categories)
        {
            _context.Categories.Add(categories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostCategories", new { id = categories.CategoryId }, categories);
        }

        // DELETE: api/Category/5
        // THIS WILL DELETE THE PRODUCT ALSO
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            var productWithSameProducts = _context.Products.Where(p => p.Category == categories.Name);

            // Remove product with the same category name
            _context.Products.RemoveRange(productWithSameProducts);

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}