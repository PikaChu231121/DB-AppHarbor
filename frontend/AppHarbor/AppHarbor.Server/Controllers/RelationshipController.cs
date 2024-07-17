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
        public IActionResult FindMySubscriber([FromForm] string token)
        {
            if (token == null || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Msg = "Invalid request data" });
            }
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User2Id equals friend.Id
                        join token1 in _dbContext.TokenIds on token equals token1.Token
                        where user.User1Id == token1.Id
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
        public IActionResult FindMySubscriber([FromForm] string token, [FromForm] string relationship)
        {

            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User2Id equals friend.Id
                        join token1 in _dbContext.TokenIds on token equals token1.Token
                        where user.User1Id == token1.Id && user.Relationship1 == relationship
                        select new
                        {
                            id = friend.Id,
                            nickname = friend.Nickname,
                            avatar = friend.Avatar,
                            state = friend.State
                        };

            var result = query.ToList();

            // 打印查询结果数量
            Console.WriteLine($"Query result count: {result.Count}");

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
        public IActionResult FindMyFollower([FromForm] string token)
        {
            var query = from user in _dbContext.Relationships
                        join friend in _dbContext.Users on user.User1Id equals friend.Id
                        join token1 in _dbContext.TokenIds on token equals token1.Token
                        where user.User2Id == token1.Id
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
        public IActionResult AddFriend([FromForm] string token, [FromForm] decimal friendId, [FromForm] string Relationship)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }
            // 找到给定token对应的用户ID
            var userId = _dbContext.TokenIds
                .Where(t => t.Token == token)
                .Select(t => t.Id)
                .FirstOrDefault();

            if(userId == friendId)
            {
                return NotFound(new
                {
                    Data = 5,
                    Msg = "Relationship has myself"
                });
            }
            // 检查是否已经存在好友关系
            var friendshipExists = _dbContext.Relationships
                .Any(r => r.User1Id == userId && r.User2Id == friendId);
            if (friendshipExists)
            {
                return NotFound(new
                {
                    Data = 4,
                    Msg = "Relationship has existed"
                });
            }


            if(!string.IsNullOrEmpty(token))
            {
                var myID = _dbContext.TokenIds.Find(token);
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
            return Ok();
        }

        [HttpPost("deletefriend")]
        public IActionResult DeleteFriend([FromForm] string token, [FromForm] decimal friendId)
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

            var user = _dbContext.Users.Find(tokenEntry.Id);
            //没有找到用户
            if (user == null)
            {
                return NotFound(new
                {
                    Data = 2,
                    Msg = "User not found"
                });
            }

            //用户状态被禁
            if (user.State == "banned")
            {
                return NotFound(new
                {
                    Data = 3,
                    Msg = "User state abnormal"
                });
            }

            var relationship = _dbContext.Relationships
                .FirstOrDefault(r => (r.User1Id == user.Id && r.User2Id == friendId) || (r.User1Id == friendId && r.User2Id == user.Id));

            //不存在关系
            if (relationship == null)
            {
                return NotFound(new
                {
                    Data = 4,
                    Msg = "Relationship not found"
                });
            }

            //删除好友关系
            _dbContext.Relationships.Remove(relationship);
            _dbContext.SaveChanges();

            //返回删除成功
            return Ok(new
            {
                Data = 1,
                Msg = "Friend deleted successfully"
            });
        }
    }
}


