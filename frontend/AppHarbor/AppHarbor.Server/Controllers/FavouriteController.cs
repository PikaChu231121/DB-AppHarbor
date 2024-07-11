using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System.Text.Json;

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
        public IActionResult GetFavourites([FromBody] TokenRequest request)
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

            var favouriteList = _dbContext.Favourites
                .Where(f => f.UserId == user.Id)
                .Select(f => new
                {
                    appId = f.ApplicationId,
                    appName = f.Name,
                    appVisibility = f.Visibility,
                    userId = f.UserId
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

    }
}
