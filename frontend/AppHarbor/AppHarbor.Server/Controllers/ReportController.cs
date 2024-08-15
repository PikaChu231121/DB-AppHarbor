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
                .Join(_dbContext.Users,
                      report => report.UserId,
                      user => user.Id,
                      (report, user) => new { report, user })
                .Join(_dbContext.Applications,
                      temp => temp.report.ApplicationId,
                      app => app.Id,
                      (temp, app) => new { temp.report, temp.user, app })
                .Join(_dbContext.Merchants,
                      temp => temp.app.MerchantId,
                      merchant => merchant.Id,
                      (temp, merchant) => new
                      {
                          reportId = temp.report.Id,
                          content = temp.report.Content,
                          applicationId = temp.report.ApplicationId,
                          MerchantId = merchant.Id,
                          userId = temp.report.UserId,
                          time = temp.report.Time,
                          userNickname = temp.user.Nickname,
                          applicationName = temp.app.Name,
                          merchantNickname = merchant.Nickname
                      })
                .OrderBy(result => result.time)
                .ToList();

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
