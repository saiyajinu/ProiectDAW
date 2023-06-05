using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;
using System.Data;
using System.Diagnostics;

namespace ProiectDAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            var fiveStarRev = db.Reviews.OrderBy(rev => rev.Id).Last(rev => rev.Rating == 5);
            var fiveStarLocId = db.Reviews.OrderBy(rev => rev.Id).Last(rev => rev.Rating == 5).LocationId;
            var location = db.Locations.Find(fiveStarLocId);
            ViewBag.review = fiveStarRev;
            ViewBag.userName = db.Users.First(u => u.Id == fiveStarRev.UserId).UserName;
            return View(location);
            
            
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            MyViewModel mvm = new MyViewModel();
            mvm.users = db.Users;
            mvm.locations = db.Locations;
            IEnumerable<Review> reviews = await db.Reviews.ToListAsync();
            mvm.reviews = reviews.Reverse();
            return View(mvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}