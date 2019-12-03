using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashionMenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : FashionMenController
    {
        public ProductsController(FashionMenDB db, IMapper mapper) : base(db, mapper)
        {}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                return await _db.products.ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                
            }
        }
    }
}