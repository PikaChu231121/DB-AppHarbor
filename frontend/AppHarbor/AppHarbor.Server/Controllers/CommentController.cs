using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        struct user_
        {
            string avatar;
            string nickname;
        }
        struct comment_
        {
            int id;
            int rating;
            string content;
            string timestamp;
            user_ user;
        }

        public CommentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Comments.ToList());
        }


        [HttpPost("getappcomment")]
        public IActionResult GetAppComment([FromBody] GetAppCommentModel getappcommentModel)
        {
            var comments = _dbContext.Comments.Where(a => a.ApplicationId == getappcommentModel.ApplicationId).ToList();
            //comment_[] resultlist = new comment_[100];
            comments.ForEach(a =>
            {
                a.User = _dbContext.Users.Find(a.UserId);
            });
            comments = comments.OrderBy(a => a.PublishTime).ToList();

            //return Ok(comments);

            if (comments == null)
            {
                return NotFound("no comments");
            }
            else
            {
                return Ok(comments);
            }


        }
    }
}
