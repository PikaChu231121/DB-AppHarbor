using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public ReportController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Reports.ToList());
        }

        [HttpPost("getreportlist")]
        public IActionResult GetReportList()
        {
            var reportList = _dbContext.Reports
                .OrderBy(r => r.Time)
                .ToList();
            return Ok(reportList);
        }

        [HttpPost("publishreport")]
        public IActionResult PublishReport([FromForm] string token, [FromForm] string Content, [FromForm] decimal ApplicationId)
        {
            //找到举报人的id
            var user = (from mytoken in _dbContext.TokenIds
                        where mytoken.Token == token
                        select new
                        {
                            mytoken.Id
                        }).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid token.");
            }

            decimal userId = user.Id;
            //判断举报应用是否存在且已经发布
            var targerapp = (from app in _dbContext.Applications
                        where app.Id == ApplicationId&&app.ReleaseState== "released"
                             select new
                        {
                                 app.Id
                             }).FirstOrDefault();

            if (targerapp == null)
            {
                return Unauthorized("Invalid targetapp.");
            }

            // 生成新的举报 ID: 找到表中最大的举报 Id 再加 1
            decimal newReportId = _dbContext.Reports
                .OrderByDescending(r => r.Id)
                .Select(r => r.Id)
                .FirstOrDefault() + 1;

            // 插入举报记录到数据库中
            var newReport = new Report
            {
                Id = newReportId,
                Content = Content,
                ApplicationId = ApplicationId,
                UserId = userId,
                Time = DateTime.Now
            };

            _dbContext.Reports.Add(newReport);
            _dbContext.SaveChanges();

            return Ok("Report published successfully.");
        }


    }
}


