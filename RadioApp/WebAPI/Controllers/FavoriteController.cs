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
    [Authorize]
    [Route("api/favorite")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        MySqlDatabase db = new MySqlDatabase();

        // GET: api/<FavoriteController>
        [HttpGet]
        public ActionResult Get()
        {
            return NotFound();
        }

        // GET api/<FavoriteController>/{id}/all
        [HttpGet("{id}/all")]
        public async Task<ActionResult<List<Favorite>>> GetAll(int id)
        {
            if(string.IsNullOrWhiteSpace(id.ToString()))
            {
                return BadRequest();
            }
            var favoritelist = await db.GetFavorites(id);
            if(favoritelist.Count == 0)
            {
                return BadRequest();
            }
            return favoritelist;
        }

        // POST api/<FavoriteController>/{id}
        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> Post(int id, [FromBody] Favorite favorite)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()) || favorite == null)
            {
                return BadRequest();
            }

            var success = await db.SaveFavorite(id, favorite);

            return success;
            
        }

        // PUT api/<FavoriteController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return NotFound();
        }

        // DELETE api/<FavoriteController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id,[FromBody] Favorite favorite)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()) || favorite == null)
            {
                return BadRequest();
            }
            var success = await db.DeleteFavorite(id,favorite);

            return success;

        }
    }
}
