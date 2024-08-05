using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanUserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public BanUserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.BanUsers.ToList());
        }

        [HttpPost("banuser")]
        public IActionResult Banuser([FromForm] string mytoken, [FromForm] decimal user_id, [FromForm] string reason)
        {
            //获得管理员id
            var query = from token in _dbContext.TokenIds
                        where mytoken == token.Token
                        select new
                        {
                            token = token.Token,
                            id = token.Id,
                        };

            var resultList = query.ToList();
            if (resultList.Count != 1)
            {
                return NotFound("user more than one");
            }
            //更新user表用户状态
            var curstate = "banned";
            var userEntity = _dbContext.Users.SingleOrDefault(use => use.Id == user_id);
            if (userEntity == null)
            {
                return NotFound("user not found");
            }
            else
            {
                userEntity.State= curstate;
                _dbContext.SaveChanges();
            }

            //添加到ban_user中
             var user = new BanUser
            {
                AdminId = resultList[0].id,
                UserId = user_id,
                Time = DateTime.Now,
                Reason = reason,
            };
            _dbContext.BanUsers.Add(user);
            _dbContext.SaveChanges();

            return Ok(user);
        }

        [HttpPost("searchbanuser")]
        public IActionResult Searchbanuser()
        {
            var query = from banuser in _dbContext.BanUsers
                        select new
                        {
                            AdminId = banuser.AdminId,
                            UserId = banuser.UserId,
                            Time = banuser.Time,
                            Reason = banuser.Reason,
                        };

            var resultList = query.ToList();

            return Ok(query);
        }
        
    }
}
