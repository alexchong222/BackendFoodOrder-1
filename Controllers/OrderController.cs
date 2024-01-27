using BackendFoodOrder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFoodOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly FoodOrderContext _context;

        public OrderController(FoodOrderContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> PutOrder(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
            {
                return BadRequest();
            }

            try
            {
                // Retrieve the existing order from the database
                var existingOrder = await _context.Orders.FindAsync(id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.Ratings = updatedOrder.Ratings;
                _context.Entry(existingOrder).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return existingOrder;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // PUT: api/Order/Complete/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Complete/{id}")]
        public async Task<ActionResult<Order>> PutOrderComplete(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
            {
                return BadRequest();
            }

            try
            {
                // Retrieve the existing order from the database
                var existingOrder = await _context.Orders.FindAsync(id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Update only the specific properties you want to modify
                existingOrder.OrderStatus = "Completed";
                existingOrder.DeliveryStatus = "Delivered";

                // Mark the entity as modified and save changes
                _context.Entry(existingOrder).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return existingOrder;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // PUT: api/Order/Cancel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Cancel/{id}")]
        public async Task<ActionResult<Order>> PutOrderCancel(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
            {
                return BadRequest();
            }

            try
            {
                // Retrieve the existing order from the database
                var existingOrder = await _context.Orders.FindAsync(id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Update only the specific properties you want to modify
                existingOrder.OrderStatus = "Cancelled";
                existingOrder.DeliveryStatus = "Cancelled";

                // Mark the entity as modified and save changes
                _context.Entry(existingOrder).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return existingOrder;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // POST: api/Order/{userid}/{totalamount}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{userid}/{totalamount}")]
        public async Task<ActionResult<Order>> PostOrder(Order order, int userid, string totalamount)
        {
            // Set properties for the Order object based on the provided parameters and default values.
            order.UserId = userid;
            order.TotalAmount = totalamount;
            order.OrderStatus = "Pending";
            order.DeliveryStatus = "Pending";
            order.Ratings = "Not given yet";
            order.DTAdded = DateTime.Today.ToString("MM-dd-yyyy");
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Retrieve data from the Cart table for the given user ID.
            var cartItems = _context.Carts.Where(c => c.UserId == userid).ToList();

            // Populate the OrderDetails table with data from the Cart table.
            foreach (var cartItem in cartItems)
            {
                if (int.TryParse(cartItem.Quantity, out int quantity) && decimal.TryParse(cartItem.Price, out decimal price))
                {
                    var orderDetail = new OrderDetails
                    {
                        OrderId = order.OrderId,
                        ProductId = cartItem.ProductId,
                        Quantity = quantity.ToString(),
                        Price = price.ToString(),
                        TotalAmount = (quantity * price).ToString(),
                        DTAdded = DateTime.Today.ToString("MM-dd-yyyy")
                };
                    _context.OrderDetails.Add(orderDetail);
                }
                else
                {
                    return NotFound();
                }
            }
            await _context.SaveChangesAsync();

            var userCartItems = _context.Carts.Where(c => c.UserId == userid);
            _context.Carts.RemoveRange(userCartItems);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }


        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}