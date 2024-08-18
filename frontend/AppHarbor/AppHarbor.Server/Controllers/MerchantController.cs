using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public MerchantController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Merchants.ToList());
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] string Nickname, [FromForm] string password)
        {
            var newuser = new Merchant()
            {
                Password = password,
                Nickname = Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "Normal"
            };

            _dbContext.Merchants.Add(newuser);
            _dbContext.SaveChanges();

            return Ok(newuser);

        }

        [HttpPost("logout")]
        public IActionResult Logout([FromForm] string token)
        {
            if (_dbContext.TokenIds.FirstOrDefault(t => t.Token == token) != null)
            {
                // 删除该token
                _dbContext.TokenIds.Where(u => u.Token == token).ExecuteDelete();
                return Ok("Logout successfully");
            }
            else
            {
                return BadRequest("Invalid token");
            }
        }

        [HttpPost("searchunbanmerchant")]
        public IActionResult SearchunbanMerchant()
        {
            var query = from merchant in _dbContext.Merchants
                        where merchant.State != "banned"
                        select new
                        {
                            Id = merchant.Id,
                            Nickname = merchant.Nickname,
                            RegisterTime = merchant.RegisterTime,
                        };

            var resultList = query.ToList();

            return Ok(resultList);
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] decimal merchant_id, [FromForm] string password)
        {
            var merchant = _dbContext.Merchants.Find(merchant_id);
            if (merchant == null)
            {
                return NotFound("商家未找到！");
            }
            if (_dbContext.BanMerchants.FirstOrDefault(i => merchant.Id == i.MerchantId) != null)
            {
                return BadRequest("商家已被封禁！");
            }
            if (merchant.Password == password)
            {
                // 删除同id的其他token
                // _dbContext.TokenIds.Where(u => u.Id == id).ExecuteDelete();

                // 创建token并保存到数据库
                var token = Guid.NewGuid().ToString();
                var tokenid = new TokenId()
                {
                    Id = merchant.Id,
                    Token = token,
                    ExpireDate = DateTime.UtcNow.AddDays(1)
                };
                _dbContext.TokenIds.Add(tokenid);

                _dbContext.SaveChanges();

                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid password");
            }
        }

        [HttpPost("getTransactions")]
        public async Task<IActionResult> GetTransactions(
     [FromForm] string token,
     [FromForm] string? search,
     [FromForm] string? applicationId,
     [FromForm] string? buyerId,
     [FromForm] string? receiverId,
     [FromForm] string? applicationName,
     [FromForm] string? buyerName,
     [FromForm] string? receiverName,
     [FromForm] string? startDate,
     [FromForm] string? endDate,
     [FromForm] int page,
     [FromForm] string? sortBy,
     [FromForm] string? sortOrder)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            const int pageSize = 10;
            var query = _dbContext.Orders
                .Where(o => _dbContext.Applications.Any(a => a.Id == o.ApplicationId && a.MerchantId == merchant.Id))
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.Id.ToString().Contains(search) ||
                    o.BuyerId.ToString().Contains(search) ||
                    o.ReceiverId.ToString().Contains(search) ||
                    o.ApplicationId.ToString().Contains(search) ||
                    _dbContext.Applications.Any(a => a.Id == o.ApplicationId && a.Name.Contains(search)) ||
                    _dbContext.Users.Any(u => u.Id == o.BuyerId && u.Nickname.Contains(search)) ||
                    _dbContext.Users.Any(u => u.Id == o.ReceiverId && u.Nickname.Contains(search))
                );
            }

            if (!string.IsNullOrEmpty(applicationId) && int.TryParse(applicationId, out int appId))
            {
                query = query.Where(o => o.ApplicationId == appId);
            }

            if (!string.IsNullOrEmpty(buyerId) && int.TryParse(buyerId, out int bId))
            {
                query = query.Where(o => o.BuyerId == bId);
            }

            if (!string.IsNullOrEmpty(receiverId) && int.TryParse(receiverId, out int rId))
            {
                query = query.Where(o => o.ReceiverId == rId);
            }

            if (!string.IsNullOrEmpty(applicationName))
            {
                query = query.Where(o => _dbContext.Applications.Any(a => a.Id == o.ApplicationId && a.Name.Contains(applicationName)));
            }

            if (!string.IsNullOrEmpty(buyerName))
            {
                query = query.Where(o => _dbContext.Users.Any(u => u.Id == o.BuyerId && u.Nickname.Contains(buyerName)));
            }

            if (!string.IsNullOrEmpty(receiverName))
            {
                query = query.Where(o => _dbContext.Users.Any(u => u.Id == o.ReceiverId && u.Nickname.Contains(receiverName)));
            }

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out DateTime start))
            {
                start = start.Date;
                query = query.Where(o => o.Time >= start);
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out DateTime end))
            {
                end = end.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.Time < end);
            }

            // 添加排序逻辑  
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "time")
                {
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Time) : query.OrderByDescending(o => o.Time);
                }
                else if (sortBy == "amount")
                {
                    query = sortOrder == "asc" ? query.OrderBy(o => o.Amount) : query.OrderByDescending(o => o.Amount);
                }
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new
                {
                    o.Id,
                    o.Time,
                    o.Amount,
                    ApplicationName = _dbContext.Applications.First(a => a.Id == o.ApplicationId).Name,
                    BuyerName = _dbContext.Users.First(u => u.Id == o.BuyerId).Nickname,
                    ReceiverName = _dbContext.Users.First(u => u.Id == o.ReceiverId).Nickname,
                    o.ApplicationId,
                    o.BuyerId,
                    o.ReceiverId
                })
                .ToListAsync();

            return Ok(new
            {
                transactions,
                totalPages
            });
        }

        [HttpPost("MerchantInfo")]
        public IActionResult MerchantInfo([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            var merchantInfo = new
            {
                id = merchant.Id,
                nickName = merchant.Nickname,
                avatar = merchant.Avatar,
                registerTime = merchant.RegisterTime,
                credit = merchant.Credit,
                state = merchant.State
            };

            return Ok(merchantInfo);
        }

        [HttpPost("updateMerchantNickName")]
        public IActionResult UpdateMerchantNickName([FromForm] decimal id,
        [FromForm] string nickName)
        {
            var merchant = _dbContext.Merchants.Find(id);
            if (merchant == null)
            {
                return NotFound("merchant not found");
            }

            merchant.Nickname = nickName;
            _dbContext.SaveChanges();

            return Ok(merchant);
        }

        [HttpPost("getCredit")]
        public IActionResult GetCredit([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            return Ok(new { Credit = merchant.Credit });
        }

        [HttpPost("incomeStat")]
        public IActionResult IncomeStat([FromBody] MerchantIncomeStatModel request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            // 获取period并转换为时间跨度
            TimeSpan periodSpan;
            switch (request.Period.ToLower())
            {
                case "week":
                    periodSpan = TimeSpan.FromDays(7);
                    break;
                case "month":
                    periodSpan = TimeSpan.FromDays(30);
                    break;
                case "year":
                    periodSpan = TimeSpan.FromDays(365);
                    break;
                default:
                    return BadRequest("Invalid period specified.");
            }

            var cutoffDate = DateTime.UtcNow.Subtract(periodSpan);

            var query = from application in _dbContext.Applications
                        join order in _dbContext.Orders on application.Id equals order.ApplicationId
                        where application.MerchantId == merchant.Id && order.Time >= cutoffDate
                        group order by new { application.Id, application.Name } into appGroup
                        orderby appGroup.Sum(o => o.Amount) descending
                        select new
                        {
                            ApplicationId = appGroup.Key.Id,
                            ApplicationName = appGroup.Key.Name,
                            TotalAmount = appGroup.Sum(o => o.Amount),
                            PurchaseCount = appGroup.Count()  // 计算购买次数
                        };

            var result = query.ToList();

            return Ok(result);
        }

        [HttpPost("withdrawCredit")]
        public IActionResult WithdrawCredit([FromBody] MerchantWithdrawModel request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }

            if (request.Amount <= 0)
            {
                return BadRequest("Invalid amount.");
            }

            if (merchant.Credit < request.Amount)
            {
                return BadRequest("Insufficient balance.");
            }

            merchant.Credit -= request.Amount;
            _dbContext.SaveChanges();

            return Ok(new { NewCredit = merchant.Credit });
        }

        [HttpPost("getApps")]
        public async Task<IActionResult> GetApps(
    [FromForm] string token,
    [FromForm] string? search,
    [FromForm] string? name,
    [FromForm] string? category,
    [FromForm] string? state,
    [FromForm] string? version,
    [FromForm] int page,
    [FromForm] string? sortBy,
    [FromForm] string? sortOrder)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == token);

            if (tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired token.");
            }

            var merchant = _dbContext.Merchants.Find(tokenEntry.Id);
            if (merchant == null)
            {
                return Unauthorized("Merchant not found.");
            }


            var query = _dbContext.Applications
                .Where(a => a.MerchantId == merchant.Id)
                .AsQueryable();

            // 全局搜索，忽略大小写
            if (!string.IsNullOrEmpty(search))
            {
                string lowerSearch = search.ToLower();
                query = query.Where(a =>
                    a.Id.ToString().Contains(lowerSearch) ||
                    a.Name.ToLower().Contains(lowerSearch) ||
                    a.Version.ToLower().Contains(lowerSearch) ||
                    a.ReleaseState.ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrEmpty(name))
            {
                string lowerName = name.ToLower();
                query = query.Where(a => a.Name.ToLower().Contains(lowerName));
            }

            if (!string.IsNullOrEmpty(category))
            {
                string lowerCategory = category.ToLower();
                query = query.Where(a => a.Category.ToLower().Contains(lowerCategory));
            }

            if (!string.IsNullOrEmpty(state))
            {
                string lowerState = state.ToLower();
                query = query.Where(a => a.ReleaseState.ToLower().Contains(lowerState));
            }

            if (!string.IsNullOrEmpty(version))
            {
                query = query.Where(a => a.Version.Contains(version));
            }
            // 从数据库加载所有结果到内存
            var appsQuery = await query
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Version,
                    a.Category,
                    a.Description,
                    a.ReleaseState,
                    a.Image,
                    a.DownloadCount,
                    a.Price,
                    a.Package,
                    a.Discount
                })
                .ToListAsync();

            // 在内存中进行版本号排序
            if (sortBy == "version")
            {
                appsQuery = sortOrder == "asc"
                    ? appsQuery.OrderBy(a => a.Version,
                    Comparer<string>.Create((v1, v2) => CompareVersions(v1, v2))).ToList()
                    : appsQuery.OrderByDescending(a => a.Version,
                    Comparer<string>.Create((v1, v2) => CompareVersions(v1, v2))).ToList();
            }
            else if (sortBy == "appId")
            {
                appsQuery = sortOrder == "asc"
                    ? appsQuery.OrderBy(a => a.Id).ToList()
                    : appsQuery.OrderByDescending(a => a.Id).ToList();
            }
            else if (sortBy == "name")
            {
                appsQuery = sortOrder == "asc"
                    ? appsQuery.OrderBy(a => a.Name).ToList()
                    : appsQuery.OrderByDescending(a => a.Name).ToList();
            }
            else if (sortBy == "releaseState")
            {
                appsQuery = sortOrder == "asc"
                    ? appsQuery.OrderBy(a => a.ReleaseState).ToList()
                    : appsQuery.OrderByDescending(a => a.ReleaseState).ToList();
            }

            const int pageSize = 10;
            var pagedApps = appsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalPages = (int)Math.Ceiling(appsQuery.Count / (double)pageSize);
            totalPages = Math.Max(totalPages, 1); // 确保至少有一页

            return Ok(new
            {
                apps = pagedApps,
                totalPages,
                merchantId = merchant.Id
            });
        }
        private int CompareVersions(string version1, string version2)
        {
            var parts1 = version1.Split('.').Select(int.Parse).ToArray();
            var parts2 = version2.Split('.').Select(int.Parse).ToArray();

            int maxLength = Math.Max(parts1.Length, parts2.Length);
            for (int i = 0; i < maxLength; i++)
            {
                int part1 = i < parts1.Length ? parts1[i] : 0; // 超出范围视为0
                int part2 = i < parts2.Length ? parts2[i] : 0;

                if (part1 > part2) return 1;
                if (part1 < part2) return -1;
            }
            return 0;
        }

        [HttpPost("updateApp")]
        public async Task<IActionResult> UpdateApp(
    [FromForm] int appId,
    [FromForm] int merchantId,
    [FromForm] string version,
    [FromForm] string? state,
    [FromForm] string description,
    [FromForm] decimal? price,
    [FromForm] decimal discount,
    [FromForm] string? image)
        {

            var app = _dbContext.Applications.FirstOrDefault(a => a.Id == appId && a.MerchantId == merchantId);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            var versionPattern = @"^(?!0)\d+\.\d+$";
            if (!Regex.IsMatch(version, versionPattern))
            {
                return BadRequest("版本号格式不正确，请重试！");
            }


            app.Version = version;
            app.ReleaseState = "test";
            app.Price = price;
            app.Description = description;
            app.Discount = discount;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Application updated successfully.", app });
        }

        [HttpPost("deleteApp")]
        public async Task<IActionResult> DeleteApp(
    [FromForm] int appId,
    [FromForm] int merchantId)
        {
            var app = await _dbContext.Applications.FirstOrDefaultAsync(a => a.Id == appId && a.MerchantId == merchantId);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            app.ReleaseState = "banned";

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Application deleted successfully." });
        }

        [HttpPost("uploadapp")]
        public IActionResult UploadApp(IFormFile file, [FromForm] int id, [FromServices] IWebHostEnvironment env)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var application = _dbContext.Applications.FirstOrDefault(a => a.Id == id);
            if (application == null)
            {
                return NotFound("Application not found.");
            }

            var checkResult = CheckUpload(file);
            if (checkResult is BadRequestObjectResult)
            {
                return checkResult;
            }

            string guid = Guid.NewGuid().ToString("N");
            string relativePath = getPath(file, env, guid);

            // 更新应用的Package字段为新上传的文件路径
            application.Package = relativePath;
            application.ReleaseState = "test";

            _dbContext.Applications.Update(application);
            _dbContext.SaveChanges();

            return Ok(new { ApplicationId = application.Id, message = "Application package uploaded and updated successfully." });
        }
        private IActionResult CheckUpload(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest(new { Code = 1001, Msg = "File not uploaded" });
            }
            return Ok();
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
        [HttpPost("upload-app-img")]
        public IActionResult UploadAppImage(IFormFile file, [FromForm] decimal id, [FromServices] IWebHostEnvironment env)
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
            app.ReleaseState = "test";
            _dbContext.Applications.Update(app);
            _dbContext.SaveChanges();

            return Ok(new { Data = relativePath, Code = 2001, Msg = "Succeeded" });
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
    }


}
