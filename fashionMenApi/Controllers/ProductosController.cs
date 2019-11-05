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
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly FashionMenDB _db;

        public ProductosController(FashionMenDB db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetAllProducts()
        {
            try
            {
                return await _db.productos.ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                
            }
        }
    }
}