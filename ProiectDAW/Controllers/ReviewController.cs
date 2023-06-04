using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }       

        public async Task<IActionResult> Index(int locationId)
        {
            
            IEnumerable<Review> reviews = await db.Reviews.Where(r => r.LocationId == locationId).ToListAsync();
            Location location = db.Locations.First(loc => loc.Id == locationId);
            ViewData["location"] = location.Name;
            ViewBag.locationId = locationId;
            MyViewModel myViewModel = new MyViewModel();
            myViewModel.users = db.Users;
            myViewModel.reviews = reviews;
            return View(myViewModel);
        }     

        public IActionResult Create(int locationId)
        {
            ViewBag.locationId = locationId;
            Location location = db.Locations.First(loc => loc.Id == locationId);
            ViewData["location"] = location.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            ApplicationUser user = db.Users.First(u => u.UserName == User.Identity.Name);
            review.UserId = user.Id;
            db.Add(review);
            db.SaveChanges();
            return RedirectToAction("Index", new { locationId = review.LocationId });
        }
    }
}

