using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public ApplicationController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Applications.ToList());
        }

        [HttpPost("getapplist")]
        public IActionResult GetAppList([FromBody] GetAppListModel getapplistModel)
        {
            if (getapplistModel.Category=="All")
            {
                var applist_category = _dbContext.Applications.ToList();
                applist_category.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });
                var resultlist = applist_category.Skip((getapplistModel.Page - 1) * 10 - 1).Take(getapplistModel.Page * 10 - 1);
                return Ok(resultlist);
            }
            else
            {
                var applist_category = _dbContext.Applications.Where(a => a.Category == getapplistModel.Category).ToList();
                applist_category.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });
                var resultlist = applist_category.Skip((getapplistModel.Page - 1) * 10 - 1).Take(getapplistModel.Page * 10 - 1);
                return Ok(resultlist);

            }

            
            //if (user == null)
            //{
            //    return NotFound("user not found");
            //}
            //if (user.Password == loginModel.Password)
            //{
            //    return Ok(user);
            //}
            //else
            //{
            //    return BadRequest("Invalid password");
            //}
        }
    }
}
