using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using fashionMenApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace fashionMenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : FashionMenController
    {
        private readonly IConfiguration _configuration;

        public UsersController(FashionMenDB db, IConfiguration configuration, IMapper mapper)
            : base(db, mapper)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            try
            {
                return _mapper.Map<List<UserResponse>>(await _db.users.ToListAsync());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpGet("{id}", Name = "GetSingleUser")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetSingleUser(int id)
        {
            try
            {
                User user = await _db.users.FirstOrDefaultAsync(u => u.id == id);

                if (user == null)
                    return NotFound();

                return _mapper.Map<UserResponse>(user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<UserResponse>> AddUser([FromBody] UserRegister user)
        {
            try
            {
                using (SHA256 sha = SHA256.Create())
                {
                    user.password = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(user.password))
                        .Select(item => item.ToString("x2")));
                }

                User dbUser = _mapper.Map<User>(user);
                _db.users.Add(dbUser);
                await _db.SaveChangesAsync();
                
                return new CreatedAtRouteResult("GetSingleUser",
                    new { dbUser.id },
                    _mapper.Map<UserResponse>(dbUser));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserLoggedIn>> LoginUser([FromBody] UserLogin userLoginModel)
        {
            try
            {
                User user = await _db.users
                    .Include(u => u.orders).ThenInclude(p => p.product)
                    .FirstOrDefaultAsync(u => u.username == userLoginModel.username);

                if (user == null)
                    return NotFound();
                
                using (SHA256 sha = SHA256.Create())
                {
                    userLoginModel.password = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(userLoginModel.password))
                        .Select(item => item.ToString("x2")));
                }

                if (user.password != userLoginModel.password)
                    return Unauthorized();

                // Leemos el secret_key desde nuestro appseting
                var secretKey = _configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                // Creamos los claims (pertenencias, características) del usuario
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    // Nuestro token va a durar un día
                    Expires = DateTime.UtcNow.AddDays(1),
                    // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                return new UserLoggedIn
                {
                    user = _mapper.Map<UserResponse>(user),
                    accessToken = tokenHandler.WriteToken(createdToken)
                };
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> EditUser(int id, [FromBody] UserRegister user)
        {
            try
            {
                using (SHA256 sha = SHA256.Create())
                {
                    user.password = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(user.password))
                        .Select(item => item.ToString("x2")));
                }

                User dbUser = _mapper.Map<User>(user);
                dbUser.id = id;
                
                _db.users.Update(dbUser);
                await _db.SaveChangesAsync();
                
                return _mapper.Map<UserResponse>(dbUser);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}