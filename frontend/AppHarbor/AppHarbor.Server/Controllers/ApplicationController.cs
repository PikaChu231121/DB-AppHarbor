using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

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

            var resultlist = _dbContext.Applications.Where(a =>
            getapplistModel.Category == "All" ? true : a.Category == getapplistModel.Category).ToList();

            resultlist.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });

            return Ok(resultlist);


           
        }

        [HttpPost("getappdetail")]
        public IActionResult GetAppDetail([FromBody] GetAppDetailModel getappdetailModel)
        {
            var app=_dbContext.Applications.Find(getappdetailModel.Id);
            if (app == null)
            {
                return NotFound("app not found");
            }
            else
            {
                return Ok(app);
            }


        }

        [HttpPost("searchapplist")]
        public IActionResult SearchAppList([FromBody] SearchAppListModel searchapplistModel)
        {

            var resultlist = _dbContext.Applications.Where(a => 
            a.Price >= searchapplistModel.Price_min && 
            a.Price <= searchapplistModel.Price_max && 
            a.Name.Contains(searchapplistModel.Content)&&
            searchapplistModel.Category=="All"? true: a.Category == searchapplistModel.Category).ToList();

            resultlist.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });
            return Ok(resultlist);


        }
    }
}
