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
            var comments = _dbContext.Comments
                .Where(a => a.ApplicationId == getappcommentModel.ApplicationId && a.State != "banned")
                .Include(c => c.User)
                .Select(c => new
                {
                    c.Id,
                    c.Score,
                    c.Content,
                    c.PublishTime,
                    Avatar = c.User.Avatar,
                    Nickname = c.User.Nickname,
                    UserId = c.User.Id
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
            if (user == null)
            {
                return BadRequest(new { success = false, msg = "User not exist!" });
            }

            // 检查用户是否拥有该应用
            var existingOrder = _dbContext.Orders
                        .FirstOrDefault(o => o.ApplicationId == postAppCommentModel.ApplicationId && o.ReceiverId == user.Id);

            if (existingOrder == null)
            {
                return BadRequest(new
                {
                    success = false,
                    msg = "你必须拥有该应用才能评论"
                });
            }

            // 检查评论内容是否为空
            if (string.IsNullOrWhiteSpace(postAppCommentModel.Content))
            {
                return BadRequest(new
                {
                    success = false,
                    msg = "评论内容不能为空"
                });
            }

            var newComment = new Comment
            {
                Content = postAppCommentModel.Content,
                Score = postAppCommentModel.Rating,
                ApplicationId = postAppCommentModel.ApplicationId,
                UserId = user.Id,
                PublishTime = DateTime.UtcNow,
                State = "active" // 默认直接发布
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

        // 获取所有评论的API
        [HttpGet("getallcomments")]
        public IActionResult GetAllComments()
        {
            var comments = _dbContext.Comments
                .Where(a => a.State != "banned")
                .Include(c => c.Application)
                .Select(c => new
                {
                    c.Id,
                    c.Score,
                    c.Content,
                    c.PublishTime,
                    Nickname = c.User.Nickname,  // 从 User 实体获取 nickname
                    AppName = c.Application.Name,  // 从 Application 实体获取应用名称
                })
                .OrderByDescending(c => c.PublishTime)
                .ToList();

            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found.");
            }
            else
            {
                return Ok(comments);
            }
        }

        // 定义请求模型以接收 commentId
        public class DeleteCommentRequest
        {
            public int CommentId { get; set; }
        }

        [HttpPost("deleteappcomment")]
        public IActionResult DeleteAppComment([FromBody] DeleteCommentRequest request)
        {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == request.CommentId);

            if (comment == null)
            {
                return NotFound(new { success = false, msg = "Comment not found!" });
            }

            comment.State = "banned";
            _dbContext.SaveChanges();

            return Ok(new { success = true, msg = "Comment banned successfully!" });
        }
    }
}
