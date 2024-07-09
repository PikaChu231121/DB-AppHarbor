using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

    }
}


