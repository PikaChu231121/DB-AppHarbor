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

        [HttpPost("adminsregister")]
        public IActionResult Register([FromForm] string Nickname, [FromForm] string password)
        {
            var newuser = new Admin()
            {
                Id = _dbContext.Admins.Select(u => u.Id).ToList().Max() + 1,
                Password = password,
                Nickname = Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "active"
            };

            _dbContext.Admins.Add(newuser);
            _dbContext.SaveChanges();

            return Ok(newuser.Id);

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

    }
}
