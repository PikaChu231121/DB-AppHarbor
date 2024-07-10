﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AppHarbor.Server.Models;
using System;

namespace AppHarbor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext _dbContext;


        public UserController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;

        }

        [HttpPost("test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.Users.ToList());
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel loginModel)
        {
            var user = _dbContext.Users.Find(loginModel.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.Password == loginModel.Password)
            {
                var token = Guid.NewGuid().ToString();
                // TODO: 保存到数据库
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid password");
            }
        }

        [HttpPost("getTransaction")]
        public IActionResult Transaction([FromBody] UserInfoModel info)
        {
            // 使用 LINQ 进行连接查询
            var query = from user in _dbContext.Users
                        join order in _dbContext.Orders on user.Id equals order.BuyerId
                        where user.Id == info.Id
                        select new
                        {
                            ApplicationName = order.Application.Name,
                            ReceiverNickName = order.Receiver.Nickname,
                            Amount = order.Amount,
                            Time = order.Time
                        };

            var result = query.ToList();

            if (!result.Any())
            {
                return NotFound("No matching records found");
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterModel registerModel)
        {
            var newuser = new User()
            {
                Id = _dbContext.Users.Select(u => u.Id).ToList().Max() + 1,
                Password = registerModel.Password,
                Nickname = registerModel.Nickname,
                Avatar = "default.png",
                RegisterTime = DateTime.Now,
                Credit = 0,
                State = "Normal"
            };

            _dbContext.Users.Add(newuser);
            _dbContext.SaveChanges();

            return Ok(newuser);

        }

        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] UserChangePasswordModel changePassworModel)
        {
            var user = _dbContext.Users.Find(changePassworModel.Id);
            if (user == null)
            {
                return NotFound("user not found");
            }
            if (user.Password == changePassworModel.OldPassword)
            {
                user.Password = changePassworModel.NewPassword;
                _dbContext.SaveChanges();
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid old password");
            }


        }

        [HttpPost("userinfo")]
        public IActionResult UserInfo([FromBody] UserInfoModel infoModel)
        {
            var user = _dbContext.Users.Find(infoModel.Id);

            if (user == null)
            {
                return NotFound("user not found");
            }
            return Ok(user);
        }

    }
}


