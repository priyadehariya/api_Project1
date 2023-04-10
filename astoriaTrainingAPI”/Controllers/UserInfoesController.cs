using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using astoriaTrainingAPI_.Models;

namespace astoriaTrainingAPI_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserInfoesController : ControllerBase
    {
        private readonly astoriaTraining80Context _context;

        public UserInfoesController(astoriaTraining80Context context)
        {
            _context = context;
        }

        //[HttpGet("UserInformation")]
        //public async Task<ActionResult<bool>> GetUserinfo(string UserName, string Password)
        //{
        //    bool User = await _context.UserInfo.AnyAsync(u => u.UserName == UserName && u.Password == Password);


        //        return User;

        //}



        // GET: api/UserInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
        {
            var userInfo = await _context.UserInfo.FindAsync(id);

            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }

        // PUT: api/UserInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// This method used to  saving new user  
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        // POST: api/UserInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("PostUserDetails")]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            try
            {

                if (_context.UserInfo.Any(e => e.UserId == userInfo.UserId))
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }

                if (
                  
                   string.IsNullOrEmpty(userInfo.FirstName) ||
                   string.IsNullOrEmpty(userInfo.LastName) ||
                   string.IsNullOrEmpty(userInfo.UserName) ||
                   string.IsNullOrEmpty(userInfo.EmailId) ||
                   string.IsNullOrEmpty(userInfo.Password) ||
                   userInfo.FirstName.Length > 100 ||
                   userInfo.LastName.Length > 100

                   )

                {
                    return BadRequest();
                }

                if (userInfo == null)
                {
                    return NotFound();
                }
                userInfo.CreationDate = DateTime.Now;
               
                _context.UserInfo.Add(userInfo);
                await _context.SaveChangesAsync();
                return Ok(userInfo);
            }
            catch (Exception InternalServerError)
            {
                throw InternalServerError;
            }


            //    _context.UserInfo.Add(userInfo);
            //    await _context.SaveChangesAsync();


            //    return CreatedAtAction("GetUserInfo", new { id = userInfo.UserId }, userInfo);
            //}

            //cach(ex){
            //    return ex
            //}
        }

        // DELETE: api/UserInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> DeleteUserInfo(int id)
        {
            var userInfo = await _context.UserInfo.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _context.UserInfo.Remove(userInfo);
            await _context.SaveChangesAsync();

            return userInfo;
        }

        private bool UserInfoExists(int id)
        {
            return _context.UserInfo.Any(e => e.UserId == id);
        }
    }
}
