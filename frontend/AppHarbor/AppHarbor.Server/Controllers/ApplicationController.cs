using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

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

        private string getPath(IFormFile file,
                                 [FromServices] IWebHostEnvironment env,
                                 string guid)
        {
            // 拼接文件路径
            string fileExt = Path.GetExtension(file.FileName);
            string fileName = guid + fileExt;
            string relativePath = Path.Combine(@"\uploads", fileName);
            string fullPath = Path.Combine(env.ContentRootPath, "Uploads", fileName);

            // 创建文件
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return relativePath.Replace("\\", "/");
        }

        private IActionResult CheckUpload(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest(new { Code = 1001, Msg = "File not uploaded" });
            }
            return Ok();
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

            resultlist.Sort((app1, app2) => { return app1.DownloadCount <= app2.DownloadCount ? 1 : -1; });

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

                resultlist.Sort((app1, app2) => { return app1.DownloadCount <= app2.DownloadCount ? 1 : -1; });
                return Ok(resultlist);
            }
            else
            {
                var resultlist = _dbContext.Applications.Where(a =>
                a.Price >= searchapplistModel.Price_min &&
                a.Price <= searchapplistModel.Price_max &&
                a.Name.Contains(searchapplistModel.Content) &&
                a.Category == searchapplistModel.Category).ToList();

                resultlist.Sort((app1, app2) => { return app1.DownloadCount <= app2.DownloadCount ? 1 : -1; });
                return Ok(resultlist);
            }

        }

        [HttpPost("uploadapp")]
        public IActionResult UploadApp(IFormFile file,
                                        [FromForm] string Name,
                                        [FromForm] string Version,
                                        [FromForm] string Description,
                                        [FromForm] decimal Price,
                                        [FromForm] string Category,
                                        [FromServices] IWebHostEnvironment env
                                        )
        {
            var checkResult = CheckUpload(file);
            if (checkResult is BadRequestObjectResult)
            {
                return checkResult;
            }

            string guid = Guid.NewGuid().ToString("N");
            string relativePath = getPath(file, env, guid);

            var application = new Application()
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
                Package = relativePath,
            };

            _dbContext.Applications.Add(application);
            _dbContext.SaveChanges();

            return Ok(new {ApplicationId = application.Id});

        }

        [HttpPost("installapp")]
        public IActionResult InstallApp([FromBody] InstallAppModel installappModel)
        {
            if (string.IsNullOrEmpty(installappModel.Token))
            {
                var failResponse = new
                {
                    msg = "No token provided!"
                };
                return Unauthorized(failResponse);
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == installappModel.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                var failResponse = new
                {
                    msg = "Invalid or expired token!"
                };
                return Unauthorized(failResponse);
            }

            var app = _dbContext.Applications.Find(installappModel.Id);
            if (app == null)
            {
                return NotFound("app not found");
            }
            else if (app.Package == "test" || app.Package == null)
            {
                return BadRequest("app no package");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == tokenEntry.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var order = _dbContext.Orders.FirstOrDefault(o => o.Type == "purchase"
                                                  && o.BuyerId == user.Id
                                                  && o.ApplicationId == installappModel.Id);
            if (order == null)
            {
                return BadRequest("User hasn't bought this app yet");
            }

            return Ok(app.Package);
        }
    }
}
