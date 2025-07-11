﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.Dominio;
using BusinessLogic.Común;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly Facade _facade;

        public WarehousesController(Facade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Authorize(Policy = nameof(Permission.CREATE_WAREHOUSES))]
        public IActionResult AddWarehouse([FromBody] AddWarehouseRequest request)
        {
            try
            {
                return Ok(_facade.AddWarehouse(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar agregar el deposito." });
            }
        }

        [HttpPut]
        [Authorize(Policy = nameof(Permission.EDIT_WAREHOUSES))]
        public IActionResult UpdateWarehouse([FromBody] UpdateWarehouseRequest request)
        {
            try
            {
                return Ok(_facade.UpdateWarehouse(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar actualizar el deposito." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(Permission.DELETE_WAREHOUSES))]
        public IActionResult DeleteWarehouse(int id)
        {
            try
            {
                return Ok(_facade.DeleteWarehouse(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar eliminar el deposito." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public ActionResult<WarehouseResponse> GetWarehouseById(int id)
        {
            try
            {
                return Ok(_facade.GetWarehouseById(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Formatter.ArgumentError(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = $"Ocurrió un error inesperado al intentar obtener el deposito con id {id}." });
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(Permission.VIEW_WAREHOUSES))]
        public ActionResult<List<WarehouseResponse>> GetAllWarehouses()
        {
            try
            {
                return Ok(_facade.GetAllWarehouses());
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado al intentar obtener los depositos." });
            }
        }

    }
}
