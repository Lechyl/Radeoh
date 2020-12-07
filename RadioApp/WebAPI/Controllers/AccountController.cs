using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DAL;
using WebAPI.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        MySqlDatabase db = new MySqlDatabase();
        // GET: api/<AccountController>
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult Get()
        {
            return NotFound();
        }

        //Login
        // GET api/<AccountController>/login
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
            var dbAccount = await db.Login(dtoAccount);
            if(dbAccount == null)
            {
                return BadRequest();
            }
            

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
    }
}
