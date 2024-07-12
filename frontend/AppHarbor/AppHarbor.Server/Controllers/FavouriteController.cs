using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public FavouriteController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Favourites.ToList());
        }


        [HttpPost("getFavourites")]
        public IActionResult GetFavourites([FromBody] TokenRequest request, string categoryFilter = null)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                var failResponse = new
                {
                    msg = "No token provided!"
                };
                return Unauthorized(failResponse);
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            var user = _dbContext.Users.Find(tokenEntry.Id);
            if (user == null)
            {
                var failResponse = new
                {
                    msg = "User not found!"
                };
                return Unauthorized(failResponse);
            }

            var query = _dbContext.Favourites
                .Where(f => f.UserId == user.Id);
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                query = query.Where(f => f.Application.Category == categoryFilter);
            }

            var favouriteList = query
                .Select(f => new
                {
                    id = f.Id,
                    applicationId = f.ApplicationId,
                    createTime = f.CreateTime,
                    visibility = f.Visibility,
                    userId = f.UserId,
                    applicationName = f.Application.Name,
                    applicationCategory = f.Application.Category,
                })
                .ToList();

            if (!favouriteList.Any())
            {
                var failResponse = new
                {
                    msg = "No favourites found for the current user!"
                };
                return NotFound(failResponse);

            }

            var data = new
            {
                Favourites = favouriteList
            };

            var jsonResponse = JsonSerializer.Serialize(data);
            return new JsonResult(jsonResponse);
        }
        
        [HttpPost("deleteFavourite")]
        public IActionResult DeleteFavourite([FromBody] DeleteFavouriteRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                var failResponse = new
                {
                    msg = "No token provided!"
                };
                return NotFound(failResponse);
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            var favourite = _dbContext.Favourites.FirstOrDefault(f => f.Id == request.Id && f.UserId == tokenEntry.Id);
            if (favourite == null)
            {
                var failResponse = new
                {
                    msg = "Favourite not found!"
                };
                return NotFound(failResponse);
            }

            _dbContext.Favourites.Remove(favourite);
            _dbContext.SaveChanges();

            var successResponse = new
            {
                success = true,
                msg = "Favourite deleted successfully!"
            };
            return Ok(successResponse);
        }

        [HttpPost("bulkDelete")]
        public IActionResult BulkDelete([FromBody] BulkDeleteRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                var failResponse = new
                {
                    msg = "No token provided!"
                };
                return NotFound(failResponse);
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            var favourites = _dbContext.Favourites
                .Where(f => request.Ids.Contains(f.Id) && f.UserId == tokenEntry.Id)
                .ToList();

            if (!favourites.Any())
            {
                var failResponse = new
                {
                    msg = "Favourites not found!"
                };
                return NotFound(failResponse);
            }

            _dbContext.Favourites.RemoveRange(favourites);
            _dbContext.SaveChanges();

            var successResponse = new
            {
                success = true,
                msg = "Favourites deleted successfully!"
            };

            return Ok(successResponse);
        }

        [HttpPost("addFavourite")]
        public IActionResult AddFavourite([FromBody] AddFavouriteRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                var failResponse = new
                {
                    msg = "No token provided!"
                };
                return NotFound(failResponse);
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            // 检查收藏夹是否已经存在该应用
            var existingFavourite = _dbContext.Favourites.FirstOrDefault(f => f.ApplicationId == request.ApplicationId && f.UserId == tokenEntry.Id);
            if (existingFavourite != null)
            {
                var failResponse = new
                {
                    msg = "Favourite already exists!"
                };
                return Conflict(failResponse);
            }
            // 新建
            var newFavourite = new Favourite
            {
                ApplicationId = request.ApplicationId,
                UserId = tokenEntry.Id,
                Visibility = "invisible",
                CreateTime = DateTime.UtcNow
            };
            _dbContext.Favourites.Add(newFavourite);
            _dbContext.SaveChanges();

            var successResponse = new
            {
                success = true,
                msg = "Favourite added successfully!"
            };
            return Ok(successResponse);
        }

    }
}
