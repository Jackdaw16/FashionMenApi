using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using fashionMenApi.Contexts;
using fashionMenApi.Models;
using fashionMenApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace fashionMenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly FashionMenDB _db;

        private readonly IConfiguration _configuration;
        //private IUserServices _userServices;

        public UsuariosController(FashionMenDB db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAllUsers()
        {
            try
            {
                return await _db.usuarios.ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpGet("{id}", Name = "GetSingleUser")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetSingleUser(int id)
        {
            try
            {
                Usuario usuario = await _db.usuarios.FirstOrDefaultAsync(u => u.id == id);

                if (usuario == null)
                    return NotFound();

                return usuario;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Usuario>> AddUser([FromBody] Usuario usuario)
        {
            try
            {
                using (SHA256 sha = SHA256.Create())
                {
                    usuario.passwd = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(usuario.passwd))
                        .Select(item => item.ToString("x2")));
                }
                _db.usuarios.Add(usuario);
                await _db.SaveChangesAsync();
                
                return new CreatedAtRouteResult("GetSingleUser", new { usuario.id }, usuario);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> LoginUser([FromBody] LoginViewModel loginModel)
        {
            try
            {
                Usuario user = await _db.usuarios
                    .FirstOrDefaultAsync(u => u.nombre_usuario == loginModel.username);

                if (user == null)
                    return NotFound();
                
                using (SHA256 sha = SHA256.Create())
                {
                    loginModel.password = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(loginModel.password))
                        .Select(item => item.ToString("x2")));
                }

                if (user.passwd != loginModel.password)
                    return Unauthorized();

                // Leemos el secret_key desde nuestro appseting
                var secretKey = _configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                // Creamos los claims (pertenencias, características) del usuario
                var claims = new[]
                {
                    new Claim("UserData", JsonConvert.SerializeObject(user)),
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
                
                return Ok(new LoggedUser
                {
                    user = user,
                    accessToken = tokenHandler.WriteToken(createdToken)
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> EditUser(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.id)
                    return BadRequest();
                
                using (SHA256 sha = SHA256.Create())
                {
                    usuario.passwd = String.Concat(sha
                        .ComputeHash(Encoding.UTF8.GetBytes(usuario.passwd))
                        .Select(item => item.ToString("x2")));
                }
                
                _db.usuarios.Update(usuario);
                await _db.SaveChangesAsync();
                
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}