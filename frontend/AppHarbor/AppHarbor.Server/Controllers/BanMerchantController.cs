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
        public IActionResult Searchbanuser()
        {
            var query = from banmerchant in _dbContext.BanMerchants
                        select new
                        {
                            AdminId = banmerchant.AdminId,
                            UserId = banmerchant.MerchantId,
                            Time = banmerchant.Time,
                            Reason = banmerchant.Reason,
                        };

            var resultList = query.ToList();

            return Ok(query);
        }
    }
}
