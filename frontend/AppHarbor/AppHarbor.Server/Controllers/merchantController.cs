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

        [HttpGet]
        public ActionResult<IEnumerable<Merchant>> Get()
        {
            return _dbContext.merchant.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Merchant> Get(int id)
        {
            var merchant = _dbContext.merchant.Find(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return merchant;
        }




        //public IActionResult GetTYRuleCodes(string? id)
        //{
        //    try
        //    {
        //        var ruleCodes = string.IsNullOrEmpty(id) ? _dbContext.user
        //            .ToList() : _dbContext.user.Where(code => code.ID == id)
        //            .ToList();

        //        return Ok(ruleCodes);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An error occurred: {ex.Message}");
        //        return BadRequest($"An error occurred while fetching data: {ex.Message}");
        //    }
        //}

    }
}


