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
            //var comments = _dbContext.Comments.Where(a => a.ApplicationId == getappcommentModel.ApplicationId).ToList();
            ////comment_[] resultlist = new comment_[100];
            //comments.ForEach(a =>
            //{
            //    a.User = _dbContext.Users.Find(a.UserId);
            //});
            //comments = comments.OrderBy(a => a.PublishTime).ToList();

            ////return Ok(comments);
            ///
            var comments = _dbContext.Comments
                .Where(a => a.ApplicationId == getappcommentModel.ApplicationId)
                .Include(c => c.User)
                .Select(c => new
                {
                    c.Id,
                    c.Score,
                    c.Content,
                    c.PublishTime,
                    Avatar = c.User.Avatar,
                    Nickname = c.User.Nickname
                })
                .OrderByDescending(c => c.PublishTime)
                .ToList();

            if (comments == null)
            {
                return NotFound("no comments");
            }
            else
            {
                return Ok(comments);
            }
        }


        [HttpPost("postappcomment")]
        public IActionResult PostAppComment([FromBody] PostAppCommentModel postAppCommentModel)
        {
            if (string.IsNullOrEmpty(postAppCommentModel.Token))
            {
                var failResponse = new
                {
                    success = false,
                    msg = "No token provided!"
                };
                return Unauthorized(failResponse);
            }
            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == postAppCommentModel.Token);
            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    success = false,
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            var user = _dbContext.Users.Find(tokenEntry.Id);
            if(user == null)
            {
                return BadRequest(new { success = false, msg = "User not exist!" });
            }

            var newComment = new Comment
            {
                Content = postAppCommentModel.Content,
                Score = postAppCommentModel.Rating,
                ApplicationId = postAppCommentModel.ApplicationId,
                UserId = user.Id,
                PublishTime = DateTime.UtcNow
            };
            _dbContext.Comments.Add(newComment);
            _dbContext.SaveChanges();

            var successResponse = new
            {
                success = true,
                msg = "Comment added successfully",
                commentId = newComment.Id
            };
            return Ok(successResponse);
        }
    }
}
