using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext _dbContext;


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


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterModel registerModel)
        {
            var newuser =new User() 
            {
                Id= _dbContext.Users.Select(u => u.Id).ToList().Max() + 1,
                Password=registerModel.Password,
                Nickname=registerModel.Nickname, 
                Avatar= "default.png",
                RegisterTime = DateTime.Now,
                Credit=0,
                State="Normal"
            };
            
            _dbContext.Users.Add(newuser);
            _dbContext.SaveChanges();
            
            return Ok(newuser);

        }

        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] UserChangePasswordModel changePassworModel)
        {
            var user = _dbContext.Users.Find(changePassworModel.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.Password == changePassworModel.OldPassword)
            {
                user.Password=changePassworModel.NewPassword;
                _dbContext.SaveChanges();
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid old password");
            }


        }

    }
}


