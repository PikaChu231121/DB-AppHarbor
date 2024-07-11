using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public RelationshipController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Relationships.ToList());
        }


        [HttpPost("findrelationship")]
        public IActionResult FindRelationship([FromForm] decimal User1Id)
        {
            var query = from user in _dbContext.Relationships
                        where user.User1Id == User1Id
                        select new
                        {
                            yourFriend= user.User2Id
                        };

            var result = query.ToList();
            //判断集合中是否有任何元素
            //如果集合中有元素，返回 true；如果集合为空，返回 false。
            if (!result.Any())
            {
                return NotFound("No matching records found");
            }

            return Ok(result);
        }

    }
}


