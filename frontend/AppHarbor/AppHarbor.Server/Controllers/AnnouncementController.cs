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

        [HttpPost("publishannouncement")]
        public IActionResult PublishAnnouncement([FromBody] PublishAnnouncementModel model)
        {
            //验证管理员 Token 并找到管理员 ID
            var adminToken = model.Token;
            var admin = (from token in _dbContext.TokenIds
                         where token.Token == adminToken
                         select new
                         {
                             token.Id
                         }).FirstOrDefault();

            if (admin == null)
            {
                return Unauthorized("Invalid token.");
            }

            decimal adminId = admin.Id;

            // 生成新的公告 ID: 找到表中最大的公告Id再加1
            decimal newAnnouncementId = _dbContext.Announcements
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefault() + 1;

            // 插入公告记录到数据库中
            var newAnnouncement = new Announcement
            {
                Id = newAnnouncementId,
                Title = model.Title,
                Content = model.Content,
                AdminId = adminId,
                PublishTime = model.PublishTime
            };

            _dbContext.Announcements.Add(newAnnouncement);
            _dbContext.SaveChanges();

            return Ok("Announcement published successfully.");
        }


    }
}
