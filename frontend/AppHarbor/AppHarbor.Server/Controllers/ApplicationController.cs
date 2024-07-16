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
            var app = _dbContext.Applications.Find(getappdetailModel.Id);
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
            if (searchapplistModel.Category == "All")
            {
                var resultlist = _dbContext.Applications.Where(a =>
                a.Price >= searchapplistModel.Price_min &&
                a.Price <= searchapplistModel.Price_max &&
                a.Name.Contains(searchapplistModel.Content)).ToList();

                resultlist.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });
                return Ok(resultlist);
            }
            else
            {
                var resultlist = _dbContext.Applications.Where(a =>
                a.Price >= searchapplistModel.Price_min &&
                a.Price <= searchapplistModel.Price_max &&
                a.Name.Contains(searchapplistModel.Content) &&
                a.Category == searchapplistModel.Category).ToList();

                resultlist.Sort((app1, app2) => { return app1.DownloadCount < app2.DownloadCount ? 1 : -1; });
                return Ok(resultlist);
            }

        }

        [HttpPost("uploadapp")]
        public IActionResult UploadApp([FromForm] string Name,
                                        [FromForm] string Version,
                                        [FromForm] string Description,
                                        [FromForm] decimal Price,
                                        [FromForm] string Category


                                        )
        {
            // if (appModel == null)
            // {
            //     return BadRequest("Invalid data.");
            // }
            Console.WriteLine($"Received parameters: Name={Name}, Version={Version}, Description={Description}, Price={Price}, Category={Category}");
            var application = new Application
            {
                Version = Version,
                MerchantId = 7,
                Name = Name,
                Category = Category,
                Description = Description,
                ReleaseState = "test",
                Image = "img_url",
                DownloadCount = 0,
                Price = Price,
            };

            _dbContext.Applications.Add(application);
            _dbContext.SaveChanges();

            return Ok(new { ApplicationId = application.Id });
        }
    }
}
