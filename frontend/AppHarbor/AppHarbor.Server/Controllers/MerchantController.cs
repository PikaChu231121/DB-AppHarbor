using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public MerchantController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Merchants.ToList());
        }


        [HttpPost("login")]
        public IActionResult Login([FromForm] decimal merchant_id, [FromForm] string password)
        {
            var merchant = _dbContext.Merchants.Find(merchant_id);
            if (merchant == null)
            {
                return NotFound("merchant not found");
            }
            if (merchant.Password == password)
            {
                // 删除同id的其他token
                // _dbContext.TokenIds.Where(u => u.Id == id).ExecuteDelete();

                // 创建token并保存到数据库
                var token = Guid.NewGuid().ToString();
                var tokenid = new TokenId()
                {
                    Id = merchant.Id,
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

        [HttpPost("getTransactions")]
        public async Task<IActionResult> GetTransactions([FromForm] string token, [FromForm] string? search, [FromForm] string? startDate, [FromForm] string? endDate, [FromForm] int page)
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

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            const int pageSize = 10;
            var query = _dbContext.Orders
                .Where(o => _dbContext.Applications.Any(a => a.Id == o.ApplicationId && a.MerchantId == merchant.Id))
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.Id.ToString().Contains(search) ||
                    o.BuyerId.ToString().Contains(search) ||
                    o.ReceiverId.ToString().Contains(search) ||
                    o.ApplicationId.ToString().Contains(search) ||
                    _dbContext.Applications.Any(a => a.Id == o.ApplicationId && a.Name.Contains(search))
                );
            }

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out DateTime start))
            {
                query = query.Where(o => o.Time >= start);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out DateTime end))
            {
                query = query.Where(o => o.Time <= end);
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new
                {
                    o.Id,
                    o.Time,
                    o.Amount,
                    ApplicationName = _dbContext.Applications.First(a => a.Id == o.ApplicationId).Name,
                    BuyerName = _dbContext.Users.First(u => u.Id == o.BuyerId).Nickname,
                    ReceiverName = _dbContext.Users.First(u => u.Id == o.ReceiverId).Nickname,
                    o.ApplicationId,
                    o.BuyerId,
                    o.ReceiverId
                })
                .ToListAsync();

            return Ok(new
            {
                transactions,
                totalPages
            });
        }
    }
}
