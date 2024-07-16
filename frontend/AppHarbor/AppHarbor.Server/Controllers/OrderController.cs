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
                            // 确保ID是唯一的，可以使用GUID或数据库自动生成的ID
                            Id = _dbContext.Orders.Select(u => u.Id).ToList().Max() + 1,
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

    }
}


