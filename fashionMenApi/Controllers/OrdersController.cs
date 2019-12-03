using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using fashionMenApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashionMenApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController: FashionMenController
    {
        public OrdersController(FashionMenDB db, IMapper mapper) : base(db, mapper)
        {}
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
        {
            try
            {
                return _mapper.Map<List<OrderResponse>>(await _db.orders.Include(o => o.product).ToListAsync());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}", Name = "GetSingleOrder")]
        public async Task<ActionResult<OrderResponse>> GetSingleOrder(int id)
        {
            try
            {
                Order order = await _db.orders
                    .Include(o => o.product)
                    .FirstOrDefaultAsync(u => u.id == id);

                if (order == null)
                    return NotFound();

                return _mapper.Map<OrderResponse>(order);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> AddOrder([FromBody] OrderCreate order)
        {
            try
            {
                Order dbOrder = _mapper.Map<Order>(order);
                User currentUser = await GetUser();

                if (currentUser == null)
                    return Unauthorized();

                dbOrder.user = currentUser;
                
                _db.orders.Add(dbOrder);
                await _db.SaveChangesAsync();
                
                return new CreatedAtRouteResult("GetSingleOrder", 
                    new {id = dbOrder.id},
                    _mapper.Map<OrderResponse>(dbOrder));
                
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            } 
        }

    }
}