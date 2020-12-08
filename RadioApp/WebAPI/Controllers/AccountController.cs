using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DAL;
using WebAPI.Helper;
using WebAPI.Models;



using Microsoft.Extensions.Options;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        MySqlDatabase db = new MySqlDatabase();
        private readonly AppSettings _appSettings;
        public AccountController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // GET: api/<AccountController>
       
        [HttpGet()]
        public ActionResult Get()
        {
            return NotFound();
        }

        //Login
        // POST api/<AccountController>/login
       // [AllowAnonymous]

        [HttpPost("login")]
        public async Task<ActionResult<DtoAccount>> Get([FromBody] Account account)
        {
            if(account == null)
            {
                return BadRequest();
            }
            var dtoAccount = new DtoAccount()
            {
                Id = 0,
                Email = account.Email,
                Username = account.Username,
                Password = account.Password
            };
            dtoAccount = await db.Login(dtoAccount);
            if (dtoAccount == null)
            {
                return BadRequest();
            }
            dtoAccount.Token = Authenticate(dtoAccount.Id);
            

            return Ok(dtoAccount);
        }

        
        //Register
        // POST api/<AccountController>/register
        [HttpPost("register")]
        public async Task<ActionResult<DtoAccount>> Post([FromBody] Account account)
        {
            if(account == null)
            {
                return BadRequest();
            }
            var dtoAccount = await db.Register(account);
            if(dtoAccount == null || dtoAccount.Id == 0)
            {
                return BadRequest();
            }
            return dtoAccount;

        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return NotFound();
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return NotFound();

        }

        private string Authenticate(int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
