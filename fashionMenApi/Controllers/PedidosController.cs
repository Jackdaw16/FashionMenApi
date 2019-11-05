using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashionMenApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PedidosController: ControllerBase
    {
        private readonly FashionMenDB _db;

        public PedidosController(FashionMenDB db)
        {
            _db = db;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetAllOrders()
        {
            try
            {
                return await _db.pedidos.ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{idPed}", Name = "GetSingleOrder")]
        public async Task<ActionResult<Pedido>> GetSingleOrder(int idPed)
        {
            try
            {
                Pedido pedido = await _db.pedidos.FirstOrDefaultAsync(u =>
                    u.idPed == idPed);

                if (pedido == null)
                    return NotFound();

                return pedido;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> AddOrder([FromBody] Pedido pedido)
        {
            try
            {
                _db.pedidos.Add(pedido);
                await _db.SaveChangesAsync();
                
                return new CreatedAtRouteResult("GetSingleOrder", 
                    new {pedido.idPed},
                    pedido);
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            } 
        } 

    }
}