using BackendFoodOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFoodOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly FoodOrderContext _context;

        public CartController(FoodOrderContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts.ToListAsync();
        }

        // GET: api/Cart/usercart/{userid}
        [HttpGet("usercart/{userid}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartsbyuserid(int userid)
        {
            var usercart = await _context.Carts
                .Where(c => c.UserId == userid)
                .ToListAsync();

            return usercart;
        }


        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cart/{productid}/{userid}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{productid}/{userid}")]
        public async Task<ActionResult<Cart>> PostCart(Cart cart, int productid, int userid)
        {
            // Check if the product is already in the cart
            var existingCartItem = _context.Carts
                .FirstOrDefault(c => c.ProductId == productid && c.UserId == userid);

            if (existingCartItem != null)
            {
                // Update quantity if the product is already in the cart
                if (int.TryParse(cart.Quantity, out int newQuantity) && int.TryParse(cart.Price, out int price))
                {
                    existingCartItem.Quantity = newQuantity.ToString();
                    existingCartItem.Price = price.ToString();
                    existingCartItem.TotalAmount = (price * newQuantity).ToString();
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetCart", new { id = existingCartItem.CartId }, existingCartItem);
                }
                else
                {
                    return BadRequest("Invalid Quantity format");
                }
            }
            else
            {
                // Create a new entry if the product is not in the cart
                cart.ProductId = productid;
                cart.UserId = userid;
                if (int.TryParse(cart.Price, out int price) && int.TryParse(cart.Quantity, out int quantity))
                {
                    cart.TotalAmount = (price * quantity).ToString();
                }
                else
                {
                    return BadRequest("Invalid Price or Quantity format");
                }
                cart.DTAdded = DateTime.Today.ToString("MM-dd-yyyy");
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
            }
        }


        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}