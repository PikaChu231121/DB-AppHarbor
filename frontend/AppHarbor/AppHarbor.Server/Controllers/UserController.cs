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

        
        [HttpPost("tokentest")]
        public IActionResult TokenTest([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var user = _dbContext.Users.Find(tokenEntry.Id);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // 返回受保护的数据
            return Ok(new { message = "This is protected data.", user });
        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Users.ToList());
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] decimal id, [FromForm] string password)
        {
            var user = _dbContext.Users.Find(id);
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

        [HttpPost("recharge")]
        public IActionResult Recharge([FromBody] UserRechargeModel info)
        {
            var user = _dbContext.Users.Find(info.Id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (info.Amount <= 0)
            {
                return BadRequest("Amount should be positive");
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //user.Credit += info.Amount;
                    _dbContext.SaveChanges();

                    // 插入充值记录到 order 表
                    var order = new Order
                    {
                        Time = DateTime.Now, // 当前时区
                        Amount = info.Amount,
                        ApplicationId = null, // 充值时为null
                        BuyerId = user.Id,
                        ReceiverId = null, // 充值时为null
                        Type = "recharge"
                    };

                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // 捕获异常，防止超大金额导致后端崩溃
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return Ok(new { user.Id, user.Nickname, user.Credit });
        }



        [HttpPost("getTransaction")]
        public IActionResult Transaction([FromBody] UserInfoModel info)
        {
            // 使用 LINQ 进行连接查询
            var query = from user in _dbContext.Users
                        join order in _dbContext.Orders on user.Id equals order.BuyerId
                        where user.Id == info.Id
                        orderby order.Time descending
                        select new
                        {
                            ApplicationName = order.Application.Name,
                            ReceiverNickName = order.Receiver.Nickname,
                            Amount = order.Amount,
                            Time = order.Time,
                            Type = order.Type
                        };

            var result = query.ToList();

            if (!result.Any())
            {
                return NotFound("No matching records found");
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] string Nickname, [FromForm] string password)
        {

            var newuser = new User()
            {
                Password = password,
                Nickname = Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "Normal"
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
                user.Password = changePassworModel.NewPassword;
                _dbContext.SaveChanges();
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid old password");
            }


        }

        [HttpPost("searchid")]
        public IActionResult SearchId([FromForm] decimal inputId)
        {
            var user = _dbContext.Users.Find(inputId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // 返回受保护的数据
            var userInfo = new
            {
                user.Id,
                user.Nickname,
                user.Avatar,
            };

            return Ok(userInfo);
        }

        [HttpPost("userinfo")]
        public IActionResult UserInfo([FromBody] TokenRequest request)
        {

            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var user = _dbContext.Users.Find(tokenEntry.Id);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // 返回受保护的数据
            var userInfo = new
            {
                user.Id,
                user.Nickname,
                user.Avatar,
                user.RegisterTime,
                user.Credit,
                user.State
            };

            return Ok(userInfo);
        }

        [HttpPost("updateUserNickname")]
        public IActionResult UpdateUserNickname([FromBody] UpdateUserNicknameModel nicknameModel)
        {
            var user = _dbContext.Users.Find(nicknameModel.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            user.Nickname = nicknameModel.NewNickname;
            _dbContext.SaveChanges();
            return Ok(user);
        }


        [HttpPost("searchunbanuser")]
        public IActionResult Searchunbanuser()
        {
            var query = from user in _dbContext.Users
                        where user.State != "banned"
                        select new
                        {
                            Id = user.Id,
                            Nickname = user.Nickname,
                            RegisterTime = user.RegisterTime,
                        };

            var resultList = query.ToList();

            return Ok(resultList);
        }

    }
}


