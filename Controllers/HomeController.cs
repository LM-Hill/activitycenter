using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get;set;}
        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>();

        public User GetUser()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
        }

        public HomeController(MyContext context)
        {
            _context = context;
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
                if(_context.Users.FirstOrDefault(usr => usr.Email == u.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use, try logging in!");
                    return View("Index");
                }
                string hash = regHasher.HashPassword(u, u.Password);
                u.Password = hash;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", u.UserId);
                return Redirect("/home");
            }
            return View("Index");
        }
        
        [HttpPost("login")]
        public IActionResult Login(LoginUser lu)
        {
            if(ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.LoginEmail);
                if(userInDB == null)
                {
                    ModelState.AddModelError("Email", "Email not found. Check your spelling?");
                    return View("Index");
                }
                var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email or Password!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("/home");
            }
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            ViewBag.User = current;
            List<Jubilee> AllJubilees = _context.Jubilees
                                                    .Include(j => j.Coordinator)
                                                    .Include(j => j.Teammates)
                                                    .ThenInclude(o => o.Participant)
                                                    .Where(j => j.JubileeTime >= DateTime.Now)
                                                    .OrderByDescending(j => j.JubileeTime)
                                                    .ToList();
            return View(AllJubilees);
        }

        [HttpGet("New")]
        public IActionResult NewJubilee()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost("jubilee/create")]
        public IActionResult CreateJubilee(Jubilee newJubilee)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                newJubilee.UserId = current.UserId;
                _context.Jubilees.Add(newJubilee);
                _context.SaveChanges();
                return RedirectToAction("Home");

            }
            return View ("NewJubilee");
        }

        [HttpGet("jubilee/{jubileeId}/delete")]
        public IActionResult DeleteJubilee(int jubileeId)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            Jubilee remove = _context.Jubilees.FirstOrDefault(m => m.JubileeId == jubileeId);
            _context.Jubilees.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("jubilee/{jubileeId}/{status}")]
        public IActionResult ToggleParty(int jubileeId, string status)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            if(status == "join")
            {
                Outing newOuting = new Outing();
                newOuting.UserId = current.UserId;
                newOuting.JubileeId = jubileeId;
                _context.Outings.Add(newOuting);
            }
            else if (status == "leave")
            {
                Outing backout = _context.Outings.FirstOrDefault(w => w.UserId == current.UserId && w.JubileeId == jubileeId);
                _context.Outings.Remove(backout);
            }
            _context.SaveChanges();
            return RedirectToAction ("Home");
        }

        [HttpGet("jubilee/{jubileeId}")]
        public IActionResult DisplayJubilee(int jubileeId)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }
            ViewBag.User = current;
            Jubilee game = _context.Jubilees.Include(j => j.Teammates)
                                            .ThenInclude(o => o.Participant)
                                            .Include(j => j.Coordinator)
                                            .FirstOrDefault(j => j.JubileeId == jubileeId);
            return View(game);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect ("/");
        }
    }
}