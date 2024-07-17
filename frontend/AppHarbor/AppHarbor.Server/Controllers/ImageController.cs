using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AppHarbor.Server.Models;
using System.Reflection.Metadata.Ecma335;

namespace AppHarbor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ImageController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private string SaveImage(IFormFile file,
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

        private IActionResult CheckImage(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest(new { Code = 1001, Msg = "File not uploaded" });
            }

            // 检查图片类型
            var mimeType = file.ContentType;
            var imageMimeTypes = new List<string>
            {
                "image/jpeg",
                "image/png"
            };
            if (!imageMimeTypes.Contains(mimeType))
            {
                return BadRequest(new { Code = 1002, Msg = "File is not supported image" });
            }

            return Ok();
        }

        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile file,
                                        [FromServices] IWebHostEnvironment env)
        {
            var checkResult = CheckImage(file);
            if (checkResult is BadRequestObjectResult)
            {
                return checkResult;
            }

            string guid = Guid.NewGuid().ToString("N");
            string relativePath = SaveImage(file, env, guid);

            return Ok(new { Data = relativePath.Replace("\\", "/"), Code = 2001, Msg = "Succeeded" });
        }

        [HttpPost("upload-personal-image")]
        public IActionResult UploadPersonalImage([FromForm] IFormFile file,
                                                [FromForm] decimal id,
                                                [FromServices] IWebHostEnvironment env)
        {
            var checkResult = CheckImage(file);
            if (checkResult is BadRequestObjectResult)
            {
                return checkResult;
            }

            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return BadRequest(new { Code = 1003, Msg = "User Not Found" });
            }

            string guid = Guid.NewGuid().ToString("N");
            string relativePath = SaveImage(file, env, guid);
            user.Avatar = relativePath;
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return Ok(new { Data = relativePath, Code = 2001, Msg = "Succeeded" });
        }

        [HttpPost("upload-app-img")]
        public IActionResult UploadAppImage([FromForm]IFormFile file, [FromForm]decimal id, [FromServices] IWebHostEnvironment env)
        {
            // 上传应用图片
            var checkResult = CheckImage(file);
            if (checkResult is BadRequestObjectResult)
            {
                return checkResult;
            }
            
            // 查找数据库中对应App的项目
            var app = _dbContext.Applications.Find(id);
            if (app == null)
            {
                return BadRequest(new { Code = 1003, Msg = "Application Not Found" });
            };

            // 保存图片并保存数据
            string guid = Guid.NewGuid().ToString("N");
            string relativePath = SaveImage(file, env, guid);
            app.Image = relativePath;
            _dbContext.Applications.Update(app);
            _dbContext.SaveChanges();

            return Ok(new { Data = relativePath, Code = 2001, Msg = "Succeeded" });
        }
    }
}
