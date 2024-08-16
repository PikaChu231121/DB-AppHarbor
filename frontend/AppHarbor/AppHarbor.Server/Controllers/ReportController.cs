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
            var reportList = (from report in _dbContext.Reports
                              join user in _dbContext.Users on report.UserId equals user.Id
                              join app in _dbContext.Applications on report.ApplicationId equals app.Id
                              join merchant in _dbContext.Merchants on app.MerchantId equals merchant.Id
                              where report.State== "reviewing"
                              orderby report.Time
                              select new
                              {
                                  reportId = report.Id,
                                  content = report.Content,
                                  applicationId = report.ApplicationId,
                                  MerchantId = merchant.Id,
                                  userId = report.UserId,
                                  time = report.Time,
                                  userNickname = user.Nickname,
                                  applicationName = app.Name,
                                  merchantNickname = merchant.Nickname
                              }).ToList();

            return Ok(reportList);
        }


        [HttpPost("publishreport")]
        public IActionResult PublishReport([FromBody] PublishReportModel model)
        {
            // 验证用户 Token 并找到用户 ID
            var userToken = model.Token;
            var user = (from token in _dbContext.TokenIds
                        where token.Token == userToken
                        select new
                        {
                            token.Id
                        }).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid token.");
            }

            decimal userId = user.Id;

            // 生成新的举报 ID: 找到表中最大的举报 Id 再加 1
            decimal newReportId = _dbContext.Reports
                .OrderByDescending(r => r.Id)
                .Select(r => r.Id)
                .FirstOrDefault() + 1;

            // 插入举报记录到数据库中
            var newReport = new Report
            {
                Id = newReportId,
                Content = model.Content,
                ApplicationId = model.ApplicationId,
                UserId = userId,
                Time = model.ReportTime,
                State = "reviewing" // 设置 State 为 "reviewing"
            };

            _dbContext.Reports.Add(newReport);
            _dbContext.SaveChanges();

            return Ok("Report published successfully.");
        }

    }
}
