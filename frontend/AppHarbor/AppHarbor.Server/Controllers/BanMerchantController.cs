using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanMerchantController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public BanMerchantController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.BanMerchants.ToList());
        }

        [HttpPost("searchbanmerchant")]
        public IActionResult Searchbanmerchant()
        {
            var query = from banmerchant in _dbContext.BanMerchants
                        join merchant in _dbContext.Merchants on banmerchant.MerchantId equals merchant.Id
                        select new
                        {
                            AdminId = banmerchant.AdminId,
                            UserId = banmerchant.MerchantId, // 使用 MerchantId 而不是 UserId
                            MerchantNickname = merchant.Nickname, // 从 Merchant 表中获取 Nickname
                            Time = banmerchant.Time,
                            Reason = banmerchant.Reason,
                        };

            var resultList = query.ToList();

            return Ok(query);
        }

        [HttpPost("banmerchant")]
        public IActionResult Banmerchant([FromForm] string mytoken, [FromForm] decimal merchant_id, [FromForm] string reason)
        {
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
                return NotFound("user more than one.");
            }

            // 更新商家表商家状态
            var curstate = "banned";
            var merchantEntity = _dbContext.Merchants.SingleOrDefault(m => m.Id == merchant_id);
            if (merchantEntity == null)
            {
                return NotFound("Merchant not found");
            }
            else
            {
                merchantEntity.State = curstate;
                _dbContext.SaveChanges();
            }

            // 添加到ban_merchant中
            var banMerchant = new BanMerchant
            {
                AdminId = resultList[0].id,
                MerchantId = merchant_id,
                Time = DateTime.Now,
                Reason = reason,
            };
            _dbContext.BanMerchants.Add(banMerchant);
            _dbContext.SaveChanges();

            return Ok(banMerchant);
        }

        [HttpPost("unbanmerchant")]
        public IActionResult Unbanmerchant([FromForm] string mytoken, [FromForm] decimal merchant_id)
        {
            //验证管理员的token
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
                return NotFound("user more than one.");
            }

            // 查找商家
            var merchantEntity = _dbContext.Merchants.SingleOrDefault(m => m.Id == merchant_id);
            if (merchantEntity == null)
            {
                return NotFound("Merchant not found");
            }
            else
            {
                // 检查商家的当前状态是否为"banned"
                if (merchantEntity.State != "banned")
                {
                    return BadRequest("Merchant is not banned");
                }

                // 更新商家状态为正常
                merchantEntity.State = "active";
                _dbContext.SaveChanges();
            }

            // 删除ban_merchant中对应的记录
            var banMerchantRecord = _dbContext.BanMerchants
                .SingleOrDefault(b => b.MerchantId == merchant_id && b.AdminId == resultList[0].id);

            if (banMerchantRecord != null)
            {
                _dbContext.BanMerchants.Remove(banMerchantRecord);
                _dbContext.SaveChanges();
            }

            return Ok(new { message = "Merchant successfully unbanned" });
        }

    }
}
