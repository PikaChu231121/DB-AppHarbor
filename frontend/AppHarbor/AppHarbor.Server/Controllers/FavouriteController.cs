using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

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

        [HttpPost("favourite")]
        public IActionResult UserFavourite([FromBody] UserFavouriteModel favouriteModel)
        {
            var favourite = _dbContext.Favourites.Find(favouriteModel.Id);

            if (favourite == null)
            {
                return NotFound("favourite not found");
            }
            return Ok(favourite);
        }

    }
}
