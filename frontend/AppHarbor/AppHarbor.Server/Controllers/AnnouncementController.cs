﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;


        public AnnouncementController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Announcements.ToList());
        }
    }
}