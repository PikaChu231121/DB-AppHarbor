﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public userController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _dbContext.user.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _dbContext.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("{id},{password}")]
        public String login(int id,string password)
        {
            var user = _dbContext.user.Find(id);
            if (user == null)
            {
                return "NotFound";
            }
            if (user.Password == password)
            {
                return "登录成功";

            }
            else
            {
                return "密码错误";
            }
            
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


