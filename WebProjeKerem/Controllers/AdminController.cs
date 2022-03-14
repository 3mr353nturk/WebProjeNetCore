using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebProjeKerem.Models;

namespace WebProjeKerem.Controllers
{
    public class AdminController : Controller
    {
        WebProjeKrmDbContext db = new WebProjeKrmDbContext();
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUsers()
        {
            var list = db.Users.ToList();
            return View(list);
        }

        public IActionResult CreateUpdateUser(int id = 0)
        {
            if (id == 0)
                return View(new Users());
            else
                return View(db.Users.Find(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdateUser(Users users, IFormFile files)
        {
            if (files != null && files.Length > 0)
            {
                var fileName = Path.GetFileName(files.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                users.Image = fileName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }

                if (users.Id == 0)
                {
                    users.Image = fileName;
                    users.IsRole = "U";
                    db.Users.Add(users);
                }                  
                else
                {
                    users.Image = fileName;
                    users.IsRole = users.IsRole;
                    db.Users.Update(users);
                }
                
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));                
            }

            return View(users);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var users = await db.Users.FindAsync(id);
            db.Users.Remove(users);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
