using OrderManagementSystem.Entities;
using OrderManagementSystem.Services;


using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetAllByUserAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            var created = await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = created.Id }, created);
        }

        [HttpGet("{id:int}", Name = nameof(GetOrderById))]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            order.Id = id;
            var updated = await _orderService.UpdateAsync(order);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
