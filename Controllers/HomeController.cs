using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewBeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NewBeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        private PasswordHasher<User> RegisterHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> LoginHasher = new PasswordHasher<LoginUser>();
     
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                u.Password = RegisterHasher.HashPassword(u,u.Password);
                dbContext.Users.Add(u);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId",u.UserId);
                return Redirect("/success");
            }
            return View("Index");
        }
        [HttpGet("login")]
        public IActionResult Login(LoginUser l)
        {
            if(ModelState.IsValid)
            {
                User logging_in_user = dbContext.Users.FirstOrDefault(u=>u.Email==l.LoginEmail);
                if(logging_in_user != null)
                {
                    var result = LoginHasher.VerifyHashedPassword(l,logging_in_user.Password,l.LoginPassword);
                    if(result ==0)
                    {
                        ModelState.AddModelError("LoginPassword", "Invalid Password");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId",logging_in_user.UserId);
                        return Redirect("/success");
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email");
                }
            }
            return View("Index");
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            List<Party> Party = dbContext.Partys
                .Include(p =>p.Planner)
                .Include(p =>p.AttendingUsers)
                .OrderBy(p =>p.StartTime).ToList();
            
            for(int i=0; i<Party.Count;i++)
            {
                if(Party[i].StartTime < DateTime.Now)
                {
                    Party.Remove(Party[i]);
                }
            }
            ViewBag.Party = Party;
            ViewBag.UserId = UserId;
            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return Redirect("/");
        }

        [HttpGet("party/new")]
        public IActionResult NewParty()
        {
            return View();
        }
        
        [HttpPost("party")]
        public IActionResult CreateParty(Party p)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                p.PlannerId = (int) UserId;
                dbContext.Partys.Add(p);
                dbContext.SaveChanges();
                return Redirect("/success");
            }
            else
            {
                return View("NewParty",p);
            }
        }

            [HttpGet("delete/{PartyId}")]
            public IActionResult Delete(int PartyId)
            {
                Party p =dbContext.Partys.FirstOrDefault(pt => pt.PartyId ==PartyId);
                dbContext.Partys.Remove(p);
                dbContext.SaveChanges();
                return Redirect("/success");
            }

            [HttpGet("view/{PartyId}")]
            public IActionResult ShowParty(int PartyId)
            {
                Party p = dbContext.Partys
                    .Include(pt => pt.Planner)
                    .Include(pt => pt.AttendingUsers)
                    .ThenInclude(pt => pt.Joiner)
                    .FirstOrDefault(pt => pt.PartyId == PartyId);
                ViewBag.Joins = p.AttendingUsers;
                return View(p);
            }
            [HttpGet("join/{PartyId}")]
            public IActionResult Join(int PartyId)
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                if(UserId == null)
                {
                    return Redirect("/");
                } 
                Join j = new Join(){
                    UserId = (int) UserId,
                    PartyId = PartyId
                };
                dbContext.Joins.Add(j);
                dbContext.SaveChanges();
                return Redirect("/success");
            }

            [HttpGet("leave/{PartyId}")]
            public IActionResult Leave(int PartyId)
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                if(UserId == null)
                {
                    return Redirect("/");
                } 
                Join join = dbContext.Joins
                    .Where(j => j.PartyId == PartyId)
                    .FirstOrDefault(j => j.UserId == (int) UserId);
                dbContext.Joins.Remove(join);
                dbContext.SaveChanges();
                return Redirect("/success");
            }
        }
    }