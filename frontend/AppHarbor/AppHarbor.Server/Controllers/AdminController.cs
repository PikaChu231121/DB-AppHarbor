using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public AdminController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Admins.ToList());
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] string Nickname, [FromForm] string password)
        {
            var newuser = new Admin()
            {
                Password = password,
                Nickname = Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "active"
            };

            _dbContext.Admins.Add(newuser);
            _dbContext.SaveChanges();

            return Ok(newuser);

        }

        [HttpPost("adminlogin")]
        public IActionResult Login([FromForm] decimal id, [FromForm] string password)
        {
            var user = _dbContext.Admins.Find(id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.Password == password)
            {
                // 删除同id的其他token
                _dbContext.TokenIds.Where(u => u.Id == id).ExecuteDelete();

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

        [HttpPost("admininfo")]
        public IActionResult AdminInfo([FromForm] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var admin = _dbContext.Admins.Find(tokenEntry.Id);
            if (admin == null)
            {
                return Unauthorized("Admin not found.");
            }

            // 返回受保护的管理员数据
            var adminInfo = new
            {
                admin.Id,
                admin.Nickname,  
                admin.Avatar,
                admin.RegisterTime,
                admin.State
            };

            return Ok(adminInfo);
        }


    }
}
