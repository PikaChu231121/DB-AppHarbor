using AppHarbor.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanCommentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BanCommentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.BanComments.ToList());
        }

        [HttpPost("bancomment")]
        public IActionResult BanComment([FromForm] string mytoken,
                                        [FromForm] decimal comment_id,
                                        [FromForm] string reason)
        {
            // 获得管理员id
            var tokenEntity = _dbContext.TokenIds.FirstOrDefault(tokenid => tokenid.Token == mytoken);
            if (tokenEntity == null)
            {
                return BadRequest("Invalid Token");
            }

            // 检查id是否为管理员
            var adminEntity = _dbContext.Admins.FirstOrDefault(admin => admin.Id == tokenEntity.Id); 
            if(adminEntity == null)
            {
                return BadRequest("Token owner is not an administrator");
            }

            // 更新comment表状态
            var curstate = "banned";
            var commentEntity = _dbContext.Comments.FirstOrDefault(c => c.Id == comment_id);
            if (commentEntity == null)
            {
                return BadRequest("Comment not found");
            }
            commentEntity.State = curstate;

            // 添加到ban_comment中
            var banComment = new BanComment
            {
                AdminId = adminEntity.Id,
                CommentId = comment_id,
                Time = DateTime.Now,
                Reason = reason,
            };
            _dbContext.BanComments.Add(banComment);
            _dbContext.SaveChanges();

            return Ok(banComment);
        }

        [HttpPost("unbancomment")]
        public IActionResult UnbanComment([FromForm] string mytoken,
                                [FromForm] decimal comment_id)
        {
            // 获得管理员id
            var tokenEntity = _dbContext.TokenIds.FirstOrDefault(tokenid => tokenid.Token == mytoken);
            if (tokenEntity == null)
            {
                return BadRequest("Invalid Token");
            }

            // 检查id是否为管理员
            var adminEntity = _dbContext.Admins.FirstOrDefault(admin => admin.Id == tokenEntity.Id);
            if (adminEntity == null)
            {
                return BadRequest("Token owner is not an administrator");
            }

            // 更新comment表用户状态
            var curstate = "active";
            var commentEntity = _dbContext.Comments.FirstOrDefault(c => c.Id == comment_id);
            if (commentEntity == null)
            {
                return BadRequest("Comment not found");
            }
            commentEntity.State = curstate;


            // 检查ban_comment表是否有该项
            var banComment = _dbContext.BanComments.FirstOrDefault(bc => bc.CommentId == comment_id);
            if (banComment == null)
            {
                return BadRequest("Comment is not banned");
            }

            // 在ban_comment表中删除
            _dbContext.BanComments.Remove(banComment);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
