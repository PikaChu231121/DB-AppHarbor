using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public UserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Users.ToList());
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel loginModel)
        {
            var user = _dbContext.Users.Find(loginModel.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.Password == loginModel.Password)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid password");
            }
        }

    }
}


