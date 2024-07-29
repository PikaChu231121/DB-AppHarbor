using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public OrderController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Orders.ToList());
        }

        [HttpPost("createneworder")]
        public IActionResult CreateNewOrder([FromForm] decimal BuyerID, [FromForm] decimal ReceiverID, [FromForm] decimal APPID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var App = _dbContext.Applications.Find(APPID);
                    if (App == null)
                    {
                        return BadRequest(new
                        {
                            Msg = "Not Found App..."
                        });
                    }

                    var recPeo = _dbContext.Users.Find(ReceiverID);
                    if (recPeo == null)
                    {
                        return BadRequest(new
                        {
                            Msg = "Not Found Receiver..."
                        });
                    }

                    var existingOrder = _dbContext.Orders
                        .FirstOrDefault(o => o.ApplicationId == APPID && o.ReceiverId == ReceiverID);

                    if (existingOrder != null)
                    {
                        return BadRequest(new
                        {
                            Msg = "Receiver already owns the app."
                        });
                    }

                    var APPAmount = App.Price;
                    var User = _dbContext.Users.Find(BuyerID);
                    if (User == null)
                    {
                        return BadRequest(new
                        {
                            Msg = "Not Found Buyer..."
                        });
                    }

                    if (APPAmount <= User.Credit)
                    {
                        var newOrder = new Order()
                        {
                            // ID的唯一性由数据库的seq序列来保证，插入数据时，不用填写ID
                            // Id = _dbContext.Orders.Select(u => u.Id).ToList().Max() + 1,
                            Time = DateTime.Now,
                            Amount = APPAmount.Value,
                            ApplicationId = APPID,
                            BuyerId = BuyerID,
                            ReceiverId = ReceiverID,
                            Type = "purchase"
                        };

                        _dbContext.Orders.Add(newOrder);
                        _dbContext.SaveChanges();

                        // 提交事务
                        transaction.Commit();

                        return Ok(newOrder);
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Msg = "Not enough money"
                        });
                    }
                }
                catch (Exception ex)
                {
                    // 如果发生异常，回滚事务
                    transaction.Rollback();
                    return StatusCode(500, new
                    {
                        Msg = "An error occurred while processing your request.",
                        Error = ex.Message
                    });
                }
            }
        }

        [HttpPost("fetchOwnApps")]
        public IActionResult FetchOwnApps([FromForm] string token)
        {
            //检查token
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
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            //找到所有拥有的应用
            var ownedAppIds = _dbContext.Orders
                .Where(o => o.ReceiverId == user.Id)
                .Select(o => o.ApplicationId)
                .Distinct()
                .ToList();

            var ownedApps = _dbContext.Applications
                .Where(a => ownedAppIds.Contains(a.Id))
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Version,
                    a.Category,
                    a.Image,
                    a.Description,
                    a.Package,
                })
                .ToList();

            //返回应用list
            return Ok(ownedApps);
        }


    }
}


