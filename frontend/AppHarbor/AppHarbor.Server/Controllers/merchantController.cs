using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class merchantController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public merchantController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        
    }
}


