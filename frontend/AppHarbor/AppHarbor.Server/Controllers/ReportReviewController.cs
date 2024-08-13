using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public ReportReviewController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.ReportReviews.ToList());
        }


        [HttpPost("getall")]
        public IActionResult Getall()
        {
            var reportList = _dbContext.ReportReviews
                .OrderBy(r => r.ReceiveTime)
                .ToList();
            return Ok(reportList);
        }

        [HttpPost("Acceptreports")]
        public IActionResult Acceptreports([FromForm] decimal id, [FromForm] string token,[FromForm] string result)
        {
            //找到举报提交时间
            var time = (from report in _dbContext.Reports
                         where report.Id == id
                        select new
                         {
                            report.Time
                         }).FirstOrDefault();

            if (time == null)
            {
                return Unauthorized("Invalid report time.");
            }

            //找到管理员的id
            var admin = (from mytoken in _dbContext.TokenIds
                        where mytoken.Token == token
                        select new
                        {
                            mytoken.Id
                        }).FirstOrDefault();

            if (admin == null)
            {
                return Unauthorized("Invalid token.");
            }
            if (result == null)
            {
                return Unauthorized("Result content can not be empty!.");
            }

            var roport_review = new ReportReview()
            {
                ReportId = id,
                AdminId = admin.Id,
                ReceiveTime = time.Time,
                ReviewTime = DateTime.Now,
                Result = result
            };
            _dbContext.ReportReviews.Add(roport_review);
            _dbContext.SaveChanges();
            return Ok("Accept reports successfully.");
        }
    }
}


