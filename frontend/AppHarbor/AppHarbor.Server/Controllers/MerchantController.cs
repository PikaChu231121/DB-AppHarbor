using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

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
                return NotFound("merchant not found");
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
            if(string.IsNullOrEmpty(request.Token))
            {
                return Unauthorized("No token provided.");
            }

            var tokenEntry = _dbContext.TokenIds.FirstOrDefault(t => t.Token == request.Token);

            if(tokenEntry == null || tokenEntry.ExpireDate <= DateTime.UtcNow)
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

        [HttpPost("withdrawCredit")]
        public IActionResult WithdrawCredit([FromBody] MerchanrWithdrawModel request)
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
                })
                .ToListAsync();

            // 在内存中进行版本号排序
            if (sortBy == "version")
            {
                appsQuery = sortOrder == "asc"
                    ? appsQuery.OrderBy(a => VersionStringToComparable(a.Version)).ToList()
                    : appsQuery.OrderByDescending(a => VersionStringToComparable(a.Version)).ToList();
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

            return Ok(new
            {
                apps = pagedApps,
                totalPages,
                merchantId = merchant.Id
            });
            /*            var totalRecords = await query.CountAsync();
                        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                        var apps = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
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
                            })
                            .ToListAsync();

                        return Ok(new
                        {
                            apps,
                            totalPages,
                            merchantId = merchant.Id
                        });*/
        }
        private static List<int> VersionStringToComparable(string version)
        {
            if (string.IsNullOrEmpty(version))
                return new List<int>(); // 返回空列表，防止空字符串的处理

            // 将版本号的每个部分转为整数
            return version.Split('.').Select(part =>
            {
                int.TryParse(part, out int num); // 尝试解析为整数
                return num;
            }).ToList();
        }

        [HttpPost("updateApp")]
        public async Task<IActionResult> UpdateApp(
    [FromForm] int appId,
    [FromForm] int merchantId,
    [FromForm] string version,
    [FromForm] string? state,
    [FromForm] string description,
    [FromForm] decimal? price)
        {

            var app = _dbContext.Applications.FirstOrDefault(a => a.Id == appId && a.MerchantId == merchantId);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            if (string.IsNullOrEmpty(version))
            {
                return BadRequest("Invalid name or version.");
            }

            app.Version = version;
            app.ReleaseState = state;
            app.Price = price;
            app.Description = description;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Application updated successfully.", app });
        }

        [HttpPost("updateAppImage")]
        public async Task<IActionResult> UpdateAppImage(
    [FromForm] string token,
    [FromForm] int appId,
    [FromForm] IFormFile image)
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

            var app = _dbContext.Applications.FirstOrDefault(a => a.Id == appId && a.MerchantId == merchant.Id);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            // Assume you have a method to save the image and return its path
            var imagePath = await SaveImageAsync(image);
            app.Image = imagePath;

            await _dbContext.SaveChangesAsync();

            return Ok(app);
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            // Implement your logic to save the image file and return the path
            // For example:
            var filePath = Path.Combine("images", Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return filePath;
        }

        [HttpPost("updateAppPackage")]
        public async Task<IActionResult> UpdateAppPackage(
    [FromForm] string token,
    [FromForm] int appId,
    [FromForm] string version,
    [FromForm] IFormFile package)
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

            var app = _dbContext.Applications.FirstOrDefault(a => a.Id == appId && a.MerchantId == merchant.Id);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            // Assume you have a method to save the package and return its path
            var packagePath = await SavePackageAsync(package);
            app.Package = packagePath;
            app.Version = version;

            await _dbContext.SaveChangesAsync();

            return Ok(app);
        }

        private async Task<string> SavePackageAsync(IFormFile package)
        {
            // Implement your logic to save the package file and return the path
            var filePath = Path.Combine("packages", Guid.NewGuid().ToString() + Path.GetExtension(package.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await package.CopyToAsync(stream);
            }
            return filePath;
        }

        [HttpPost("deleteApp")]
        public async Task<IActionResult> DeleteApp(
    [FromForm] string token,
    [FromForm] int appId)
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

            var app = _dbContext.Applications.FirstOrDefault(a => a.Id == appId && a.MerchantId == merchant.Id);
            if (app == null)
            {
                return NotFound("Application not found.");
            }

            _dbContext.Applications.Remove(app);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Application deleted successfully." });
        }

    }
}
