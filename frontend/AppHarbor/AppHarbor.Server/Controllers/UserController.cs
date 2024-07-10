using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext _dbContext;


        public UserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpGet("tokentest")]
        public IActionResult TokenTest(string token)
        {
            // 必须拥有合理token才能返回OK
            var tokenid = _dbContext.TokenIds.Find(token);
            if (tokenid == null)
            {
                return BadRequest("Invalid Token!");
            }
            else
            {
                var user = tokenid.IdNavigation;
                return Ok(user);
            }
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
                // 创建token并保存到数据库
                var token = Guid.NewGuid().ToString();
                var tokenid = new TokenId()
                {
                    Id = user.Id,
                    Token = token,
                    ExpireDate = DateTime.UtcNow.AddDays(1)
                };
                _dbContext.TokenIds.Add(tokenid);
                _dbContext.SaveChanges();

                return Ok(token);
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

        [HttpPost("getTransection")]
        public IActionResult Transaction([FromBody] UserInfoModel info)
        {
            var user = _dbContext.Users.Find(info.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var orders = _dbContext.Orders
                .Where(o => o.BuyerId == info.Id)
                .Select(o => new
                {
                    o.Id,
                    o.Time,
                    o.Amount,
                    o.ApplicationId,
                    o.BuyerId,
                    o.ReceiverId
                })
                .ToList();

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the specified buyer.");
            }

            return Ok(orders);
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

        [HttpPost("userinfo")]
        public IActionResult UserInfo([FromBody] UserInfoModel infoModel)
        {
            var user = _dbContext.Users.Find(infoModel.Id);
            
            if (user == null)
            {
                return NotFound("user not found");
            }
            return Ok(user);
        }

    }
}


