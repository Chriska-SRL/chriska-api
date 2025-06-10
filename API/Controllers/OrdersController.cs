using Microsoft.AspNetCore.Mvc;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.Dominio;

namespace API.Controllers
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
            try
            {
                _facade.AddOrder(request);
                return Ok(new { message = "Orden agregada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            try
            {
                _facade.UpdateOrder(request);
                return Ok(new { message = "Orden actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteOrder([FromBody] DeleteOrderRequest request)
        {
            try
            {
                _facade.DeleteOrder(request);
                return Ok(new { message = "Orden eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Puedes implementar esto más adelante
        //[HttpGet("{id}")]
        //public IActionResult GetOrderById(int id)
        //{
        //    try
        //    {
        //        var order = _facade.GetOrderById(id);
        //        return Ok(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(new { error = ex.Message });
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetAllOrders()
        //{
        //    try
        //    {
        //        var orders = _facade.GetAllOrders();
        //        return Ok(orders);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        [HttpPost("item")]
        public IActionResult AddOrderItem([FromBody] AddOrderItemRequest orderItem)
        {
            try
            {
                _facade.AddOrderItem(orderItem);
                return Ok(new { message = "Item agregado correctamente a la orden" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("item/{id}")]
        public IActionResult GetOrderItemById(int id)
        {
            try
            {
                var item = _facade.GetOrderItemById(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
