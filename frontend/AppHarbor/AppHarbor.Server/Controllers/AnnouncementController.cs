using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public AnnouncementController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Announcements.ToList());
        }

        [HttpPost("getannouncementlist")]
        public IActionResult GetAnnouncementList()
        {
            // 从数据库中获取所有公告
            var announcementList = _dbContext.Announcements
                .OrderBy(a => a.PublishTime) // 根据发布时间排序
                .ToList();

            // 返回公告列表
            return Ok(announcementList);
        }
    }
}
