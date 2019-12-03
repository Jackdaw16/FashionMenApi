using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace fashionMenApi.Controllers
{
    public class FashionMenController : ControllerBase
    {
        protected readonly FashionMenDB _db;
        protected readonly IMapper _mapper;

        public FashionMenController(FashionMenDB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        protected async Task<User> GetUser()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
                return null;

            int userId;
            try
            {
                userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.Name).Value);
            }
            catch (Exception e)
            {
                return null;
            }

            return await _db.users.FindAsync(userId);
        }
    }
}