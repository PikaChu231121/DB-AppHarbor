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
            // 从数据库中获取所有公告并与管理员表连接
            var announcementList = (from announcement in _dbContext.Announcements
                                    join admin in _dbContext.Admins on announcement.AdminId equals admin.Id
                                    orderby announcement.PublishTime descending// 根据发布时间排序
                                    select new
                                    {
                                        announcement.Id,
                                        announcement.Title,
                                        announcement.Content,
                                        announcement.PublishTime,
                                        AdminId = admin.Id,
                                        AdminNickname = admin.Nickname // 添加管理员的昵称
                                    }).ToList();

            // 返回公告列表
            return Ok(announcementList);
        }


        [HttpPost("publishannouncement")]
        public IActionResult PublishAnnouncement([FromForm] string token, [FromForm] string title, [FromForm] string content)
        {
            //验证管理员 Token 并找到管理员 ID
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
                Title = title,
                Content = content,
                AdminId = adminId,
                PublishTime = DateTime.Now,
            };

            _dbContext.Announcements.Add(newAnnouncement);
            _dbContext.SaveChanges();

            return Ok("Announcement published successfully.");
        }


    }
}
