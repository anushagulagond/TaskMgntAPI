using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskMgntAPI.Models;
using TaskMgntAPI.DTO;

namespace TaskMgntAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly TaskMgntDbContext _context;

        public AspNetUsersController(TaskMgntDbContext context)
        {
            _context = context;
        }

        // GET: api/AspNetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetAspNetUsers()
        {
            return await _context.AspNetUsers.ToListAsync();
        }

        // GET: api/AspNetUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUser>> GetAspNetUser(string id)
        {
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);

            if (aspNetUser == null)
            {
                return NotFound();
            }

            return aspNetUser;
        }

        // PUT: api/AspNetUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUser(string id, AspNetUserDTO aspNetUserDTO)
        {
            AspNetUser aspNetUser=new AspNetUser();
            aspNetUser.Id = aspNetUserDTO.Id;
            aspNetUser.UserName = aspNetUserDTO.UserName;
            aspNetUser.PhoneNumber= aspNetUserDTO.PhoneNumber;
            aspNetUser.Email= aspNetUserDTO.Email;
            aspNetUser.PasswordHash= aspNetUserDTO.PasswordHash;
            aspNetUser.UserRole = aspNetUserDTO.UserRole;
          

            if (id != aspNetUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        // POST: api/AspNetUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AspNetUser>> PostAspNetUser(AspNetUserDTO aspNetUserDTO)
        {
            AspNetUser aspNetUser = new AspNetUser();
            aspNetUser.Id = aspNetUserDTO.Id;
            aspNetUser.UserName = aspNetUserDTO.UserName;
            aspNetUser.PhoneNumber = aspNetUserDTO.PhoneNumber;
            aspNetUser.Email = aspNetUserDTO.Email;
            aspNetUser.PasswordHash = aspNetUserDTO.PasswordHash;
            aspNetUser.UserRole = aspNetUserDTO.UserRole;

            _context.AspNetUsers.Add(aspNetUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUser", new { id = aspNetUser.Id }, aspNetUser);
        }

        // DELETE: api/AspNetUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAspNetUser(string id)
        {
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }




   
    }
}
