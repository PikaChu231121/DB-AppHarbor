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

            resultlist = resultlist.OrderByDescending(app => app.DownloadCount).ToList();
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
            if (searchapplistModel.Category=="All")
            {
                var resultlist = _dbContext.Applications.Where(a =>
                a.Price >= searchapplistModel.Price_min &&
                a.Price <= searchapplistModel.Price_max &&
                a.Name.Contains(searchapplistModel.Content)).ToList();
                resultlist = resultlist.OrderByDescending(app => app.DownloadCount).ToList();
                return Ok(resultlist);
            }
            else
            {
                var resultlist = _dbContext.Applications.Where(a => 
                a.Price >= searchapplistModel.Price_min &&
                a.Price <= searchapplistModel.Price_max && 
                a.Name.Contains(searchapplistModel.Content) && 
                a.Category==searchapplistModel.Category).ToList();

                resultlist = resultlist.OrderByDescending(app => app.DownloadCount).ToList();

                return Ok(resultlist);
            }

        }

        [HttpPost("uploadapp")]
        public async Task<IActionResult> UploadApp([FromBody] UploadAppModel appModel)
        {
            if (appModel == null)
            {
                return BadRequest("Invalid data.");
            }

            var application = new Application
            {
                Version = appModel.Version,
                MerchantId = appModel.MerchantId,
                Name = appModel.Name,
                Category = appModel.Category,
                Description = appModel.Description,
                ReleaseState = appModel.ReleaseState,
                Image = appModel.Image,
                DownloadCount = 0,
                Price = appModel.Price,
            };

            _dbContext.Applications.Add(application);
            await _dbContext.SaveChangesAsync();

            return Ok(new { ApplicationId = application.Id });
        }
    }
}
