using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;

namespace API.ControllerS
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly Facade _facade;

        public OrdersController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] AddOrderRequest request)
        {
            _facade.AddOrder(request);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            _facade.UpdateOrder(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var request = new DeleteOrderRequest { Id = id };
            _facade.DeleteOrder(request);
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> GetOrderById(int id)
        {
            var response = _facade.GetOrderById(id);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<OrderResponse>> GetAllOrders()
        {
            var response = _facade.GetAllOrders();
            return Ok(response);
        }

        [HttpPost("items")]
        public IActionResult AddOrderItem([FromBody] OrderItem orderItem)
        {
            _facade.AddOrderItem(orderItem);
            return Ok();
        }

        [HttpGet("items/{id}")]
        public ActionResult<OrderItemResponse> GetOrderItemById(int id)
        {
            var response = _facade.GetItemOrderById(id);
            return Ok(response);
        }
    }
}
