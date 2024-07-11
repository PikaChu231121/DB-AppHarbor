using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public RelationshipController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Relationships.ToList());
        }


        [HttpPost("findrelationship")]
        public IActionResult FindRelationship([FromForm] decimal User1Id)
        {
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User2Id equals friend.Id 
                        where user.User1Id == User1Id
                        select new
                        {
                            id=friend.Id,
                            nickname=friend.Nickname,
                            avatar=friend.Avatar,
                            state=friend.State
                        };

            var result = query.ToList();
            //判断集合中是否有任何元素
            //如果集合中有元素，返回 true；如果集合为空，返回 false。
            if (!result.Any())
            {
                return NotFound("No matching records found");
            }

            return Ok(result);
        }

        [HttpPost("addfriend")]
        public IActionResult AddFriend([FromForm] decimal Id)
        {
            var user = _dbContext.Users.Find(Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.State == "")
            {
                return NotFound("user state abnormal");
            }
            else
            {

            }



            var newuser = new User()
            {
                Id = _dbContext.Users.Select(u => u.Id).ToList().Max() + 1,
                Password = registerModel.Password,
                Nickname = registerModel.Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "Normal"
            };

            var result = query.ToList();
            //判断集合中是否有任何元素
            //如果集合中有元素，返回 true；如果集合为空，返回 false。
            if (!result.Any())
            {
                return NotFound("No matching records found");
            }

            return Ok(result);
        }

    }
}


