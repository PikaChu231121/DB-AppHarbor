using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.Http.HttpResults;

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
        [HttpPost("gethandlelist")]
        public IActionResult GetHandlelist()
        {
            var reportList = (from reportreview in _dbContext.ReportReviews
                              join report in _dbContext.Reports on reportreview.ReportId equals report.Id
                              join app in _dbContext.Applications on report.ApplicationId equals app.Id
                              where report.State != "reviewing"
                              orderby report.Time
                              select new
                              {
                                  applicationId = report.ApplicationId,
                                  merchantId=reportreview.AdminId,
                                  state= report.State,
                                  result=reportreview.Result,
                                  time=reportreview.ReviewTime,
                              }).ToList();

            return Ok(reportList);
        }

        [HttpPost("acceptreports")]
        public IActionResult Acceptreports([FromForm] decimal id, [FromForm] string token,[FromForm] string result)
        {
            //找到举报提交时间和应用id
            var reportdetail = (from report in _dbContext.Reports
                         where report.Id == id
                        select new
                         {
                            report.Time,
                            report.ApplicationId
                         }).FirstOrDefault();

            if (reportdetail == null)
            {
                return Unauthorized("Invalid report.");
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

            var roport_review = new ReportReview()
            {
                ReportId = id,
                AdminId = admin.Id,
                ReceiveTime = reportdetail.Time,
                ReviewTime = DateTime.Now,
                Result = result
            };
            _dbContext.ReportReviews.Add(roport_review);


            var targetreport = _dbContext.Reports.FirstOrDefault(r => r.Id == id);
            if (targetreport != null)
            {
                targetreport.State = "accept";

            }
            else
            {
                return Unauthorized("Invalid targetreport.");
            }

            var targetapp = _dbContext.Applications.FirstOrDefault(r => r.Id == reportdetail.ApplicationId);
            if (targetapp != null)
            {
                targetapp.ReleaseState = "withdrawn";
                _dbContext.SaveChanges();

            }
            else
            {
                return Unauthorized("Invalid targetreport.");
            }
            _dbContext.SaveChanges();
            return Ok("Accept reports successfully.");
        }
        [HttpPost("refusereports")]
        public IActionResult Refusereports([FromForm] decimal id, [FromForm] string token, [FromForm] string result)
        {
            //找到举报提交时间和应用id
            var reportdetail = (from report in _dbContext.Reports
                                where report.Id == id
                                select new
                                {
                                    report.Time,
                                    report.ApplicationId
                                }).FirstOrDefault();

            if (reportdetail == null)
            {
                return Unauthorized("Invalid report.");
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

            var roport_review = new ReportReview()
            {
                ReportId = id,
                AdminId = admin.Id,
                ReceiveTime = reportdetail.Time,
                ReviewTime = DateTime.Now,
                Result = result
            };
            _dbContext.ReportReviews.Add(roport_review);


            var targetreport = _dbContext.Reports.FirstOrDefault(r => r.Id == id);
            if (targetreport != null)
            {
                targetreport.State = "refuse";

            }
            else
            {
                return Unauthorized("Invalid targetreport.");
            }

            _dbContext.SaveChanges();
            return Ok("Refuse reports successfully.");
        }
    }
}


