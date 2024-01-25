using BackendFoodOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFoodOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly FoodOrderContext _context;

        public OrderDetailsController(FoodOrderContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetails()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetails>> GetOrderDetailsById(int id)
        {
            var orderdetail = await _context.OrderDetails.FindAsync(id);

            if (orderdetail == null)
            {
                return NotFound();
            }

            return orderdetail;
        }

        // GET: api/OrderDetails/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetOrderDetailsByOrderId(int id)
        {
            var orderDetails = await _context.OrderDetails
                                                .Where(od => od.OrderId == id)
                                                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return orderDetails;
        }


        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetails(int id, OrderDetails orderdetail)
        {
            if (id != orderdetail.OrderDetailsId)
            {
                return BadRequest();
            }

            _context.Entry(orderdetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetails>> PostOrderDetails(OrderDetails orderdetail)
        {
            orderdetail.DTAdded = DateTime.Today.ToString("MM-dd-yyyy");
            _context.OrderDetails.Add(orderdetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetails", new { id = orderdetail.OrderDetailsId }, orderdetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetails(int id)
        {
            var orderdetail = await _context.OrderDetails.FindAsync(id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderdetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailsId == id);
        }
    }
}