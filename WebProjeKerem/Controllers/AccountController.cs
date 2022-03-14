using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebProjeKerem.Models;

namespace WebProjeKerem.Controllers
{
    public class AccountController : Controller
    {
        WebProjeKrmDbContext db = new WebProjeKrmDbContext();
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users users, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.Email == users.Email && x.Password == users.Password).FirstOrDefault();
                if (user == null)
                {
                    //Add logic here to display some message to user    
                    ViewBag.Message = "Böyle bir kullanıcı yok.";
                    return View(users);
                }
                else
                {
                    //A claim is a statement about a subject by an issuer and    
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.    
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.IsRole)
                        //new Claim("FavoriteDrink", "Tea")
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        Redirect(ReturnUrl);
                    }

                    _httpContextAccessor.HttpContext.Session.SetString("Mail", user.Email);
                    _httpContextAccessor.HttpContext.Session.SetString("Ad", user.Name);
                    _httpContextAccessor.HttpContext.Session.SetString("Soyad", user.Surname);

                    return RedirectToAction("Index", "Admin");
                }
            }

            return View(users);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Users users, IFormFile files)
        {
            if (files != null && files.Length > 0)
            {
                var fileName = Path.GetFileName(files.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                users.Image = filePath;

                users.IsRole = "U";
                db.Users.Add(users);
                db.SaveChanges();

                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    files.CopyToAsync(fileSrteam);
                }
            }
            
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return RedirectToAction("Login", "Account");
        }
    }
}
