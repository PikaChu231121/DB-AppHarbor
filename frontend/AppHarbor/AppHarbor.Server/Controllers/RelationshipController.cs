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


        [HttpPost("findall")]
        public IActionResult FindMySubscriber([FromForm] TokenRequest request)
        {
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User2Id equals friend.Id
                        join token in _dbContext.TokenIds on request.Token equals token.Token
                        where user.User1Id == token.Id
                        select new
                        {
                            id = friend.Id,
                            nickname = friend.Nickname,
                            avatar = friend.Avatar,
                            state = friend.State
                        };

            var result = query.ToList();

            if (!result.Any())
            {
                return NotFound(new { Msg = "No matching records found" });
            }

            return Ok(new
            {
                Msg = "successful!",
                data = result
            });
        }

        [HttpPost("findmysubscriber")]
        public IActionResult FindMySubscriber([FromForm] TokenRequest request, [FromForm] string relationship)
        {
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User2Id equals friend.Id
                        join token in _dbContext.TokenIds on request.Token equals token.Token
                        where user.User1Id == token.Id && user.Relationship1 == relationship
                        select new
                        {
                            id = friend.Id,
                            nickname = friend.Nickname,
                            avatar = friend.Avatar,
                            state = friend.State
                        };

            var result = query.ToList();

            if (!result.Any())
            {
                return NotFound(new { Msg = "No matching records found" });
            }

            return Ok(new
            {
                Msg = "successful!",
                data = result
            });
        }


        [HttpPost("findmyfollower")]
        public IActionResult FindMyFollower([FromForm] TokenRequest request)
        {
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User1Id equals friend.Id
                        join token in _dbContext.TokenIds on request.Token equals token.Token
                        where user.User2Id == token.Id
                        select new
                        {
                            id = friend.Id,
                            nickname = friend.Nickname,
                            avatar = friend.Avatar,
                            state = friend.State
                        };

            var result = query.ToList();
            //判断集合中是否有任何元素
            //如果集合中有元素，返回 true；如果集合为空，返回 false。
            if (!result.Any())
            {
                return NotFound(new
                {
                    Data = 2,
                    Msg = "No matching records found"
                });
            }

            return Ok(new
            {
                data = 1,
                Msg = result
            });
        }

        [HttpPost("addfriend")]
        public IActionResult AddFriend([FromForm] TokenRequest request, [FromForm] decimal friendId, [FromForm] string Relationship)
        {
            
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }
            else
            {
                var myID = _dbContext.TokenIds.Find(request.Token);
                var user = _dbContext.Users.Find(myID.Id);
                if (user == null)
                {
                    return NotFound(new
                    {
                        Data = 2,
                        Msg = "user not found"
                    });
                }
                if (user.State == "banned")
                {
                    return NotFound(new
                    {
                        Data = 3,
                        Msg = "user state abnormal"
                    });
                }
                else
                {
                    var friend = new Relationship()
                    {
                        User1Id = myID.Id,
                        User2Id = friendId,
                        CreateTime = DateTime.Now,
                        Relationship1 = Relationship
                    };
                    _dbContext.Relationships.Add(friend);
                    _dbContext.SaveChanges();
                    return Ok(new
                    {
                        data = 1,
                        Msg = friend
                    });
                }
            }
        }
    }
}


