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
            try
            {
                // 从数据库中获取所有举报
                var reportList = _dbContext.Reports
                    .OrderBy(r => r.Time) // 根据举报时间排序
                    .ToList();

                // 返回举报列表
                return Ok(reportList);
            }
            catch (Exception ex)
            {
                // 返回错误响应
                return StatusCode(500, "服务器内部错误");
            }
        }

        [HttpPost("publishreport")]
        public IActionResult PublishReport([FromBody] PublishReportModel model)
        {
            try
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
                    Time = model.ReportTime
                };

                _dbContext.Reports.Add(newReport);
                _dbContext.SaveChanges();

                return Ok("Report published successfully.");
            }
            catch (Exception ex)
            {
                // 返回错误响应
                return StatusCode(500, "服务器内部错误");
            }
        }


    }
}


