using ApiCrudServer.Data;
using ApiCrudServer.Models;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudServer.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public UserController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if(_dbContext.Users == null)
            {
                return NotFound();
            }
            var user = await _dbContext.Users.Include(p => p.UserProfile).ToListAsync();
            return Ok(user);
        }
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<User>> GetUserId(int id)
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            var user = await _dbContext.Users
                    .Include(u => u.UserProfile) 
                    .FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }
        [HttpPost, Authorize]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Password = createUserDto.Password,
                Email = createUserDto.Email,
                IsActive = createUserDto.IsActive,
                UserProfile = new UserProfile
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    PhoneNumber = createUserDto.PhoneNumber
                }
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserId), new { id = user.ID }, null);  
        }
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _dbContext.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.ID == id);

            if (existingUser == null)
            {
                return NotFound(); 
            }
            existingUser.Username = updateUserDto.Username;
            existingUser.Password = updateUserDto.Password;
            existingUser.Email = updateUserDto.Email;
            existingUser.IsActive = updateUserDto.IsActive;

            if (existingUser.UserProfile != null)
            {
                // Update properties only if the corresponding values in the DTO are not null
                existingUser.UserProfile.FirstName = updateUserDto.FirstName;
                existingUser.UserProfile.LastName = updateUserDto.LastName;
                existingUser.UserProfile.PhoneNumber = updateUserDto.PhoneNumber;
            }
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
