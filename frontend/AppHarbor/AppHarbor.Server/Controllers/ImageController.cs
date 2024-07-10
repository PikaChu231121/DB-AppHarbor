using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AppHarbor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
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

            return relativePath;
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile file,
                                        [FromServices] IWebHostEnvironment env)
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

            string guid = Guid.NewGuid().ToString("N");
            string relativePath = SaveImage(file, env, guid);

            return Ok(new { Data = relativePath.Replace("\\", "/"), Code = 2001, Msg = "Succeeded" });
        }
    }
}
