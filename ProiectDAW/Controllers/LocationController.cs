using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _env;


        public LocationController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index()
        {
            var locations = await db.Locations.ToListAsync();
            return View(locations);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await db.Locations.FirstOrDefaultAsync(m => m.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            ViewBag.locUserId = location.UserId;
            ViewBag.actualUserId = db.Users.First(u => u.UserName == User.Identity.Name).Id;
            return View(location);
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Create(Location location, IFormFile locationImage)
        {
            location.User = db.Users.Find(_userManager.GetUserId(User));
            location.UserId = _userManager.GetUserId(User);
            if (locationImage != null && locationImage.Length > 0)  
            {
                var storagePath = Path.Combine(
                    _env.WebRootPath,
                    "images",
                    locationImage.FileName);
                var databaseFileName = "/images/" + locationImage.FileName;
                using(var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await locationImage.CopyToAsync(fileStream);
                }
                location.PhotoUrl = databaseFileName;
            }


            if (ModelState.IsValid) 
            {
                
                db.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(location);
            
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Edit(int? locationId)
        {
            if (locationId == null || locationId == 0) { 
                return NotFound();
            }
            var locationFromDb = await db.Locations.FirstOrDefaultAsync(loc => loc.Id == locationId);
            if (locationFromDb == null)
            {
                return NotFound();
            }
            ViewData["locationId"] = locationId;
            return View(locationFromDb);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int locationId, Location location, IFormFile locationImage)
        {
            var id = locationId;
            location.User = db.Users.First(u => u.UserName == User.Identity.Name);
            location.Id = id;
            if (locationImage.Length > 0)
            {
                var storagePath = Path.Combine(
                    _env.WebRootPath,
                    "images",
                    locationImage.FileName);
                var databaseFileName = "/images/" + locationImage.FileName;
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await locationImage.CopyToAsync(fileStream);
                }
                location.PhotoUrl = databaseFileName;
            }
            if (ModelState.IsValid)
            {
                db.Update(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int? locationId)
        {
            if (locationId == null || locationId == 0)
            {
                return NotFound();
            }
            var locationFromDb = await db.Locations.FirstOrDefaultAsync(loc => loc.Id == locationId);
            if (locationFromDb == null)
            {
                return NotFound();
            }
            ViewData["locationId"] = locationId;
            return View(locationFromDb);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int locationId)
        {
            var location = db.Locations.Find(locationId);
            if(location == null)
            {
                return NotFound();
            }
            db.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
