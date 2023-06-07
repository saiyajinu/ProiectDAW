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
            Review fiveStarRev;
            int fiveStarLocId;
            Location location;
            if (db.Reviews.FirstOrDefault(r => r.Rating == 5) != null)
            {
                fiveStarRev = db.Reviews.OrderBy(rev => rev.Id).Last(rev => rev.Rating == 5);
                fiveStarLocId = db.Reviews.OrderBy(rev => rev.Id).Last(rev => rev.Rating == 5).LocationId;
                location = db.Locations.Find(fiveStarLocId);
                ViewBag.review = fiveStarRev;
                ViewBag.userName = db.Users.First(u => u.Id == fiveStarRev.UserId).UserName;
                return View(location);
            }
            return View(null);
            
            
            
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            MyViewModel mvm = new MyViewModel();
            IEnumerable<ApplicationUser> users = await db.Users.ToListAsync();
            mvm.usersIEn = users;

            IEnumerable<Location> locations = await db.Locations.ToListAsync();
            mvm.locationsIEn = locations.Reverse();

            IEnumerable<Review> reviews = await db.Reviews.ToListAsync();
            mvm.reviewsIEn = reviews.Reverse();
            mvm.users = db.Users;
            mvm.locations = db.Locations;

            Dictionary<string, bool> isUserAdminDic = new Dictionary<string, bool>();

            foreach(var user in users)
            {
                isUserAdminDic[user.Id] = false;
            }
            foreach (ApplicationUser user in await _userManager.GetUsersInRoleAsync("Admin"))
            {
                isUserAdminDic[user.Id] = true;
            }
            ViewBag.dic = isUserAdminDic;

            return View(mvm);
        }

        public async Task<IActionResult> SetAdmin(string userId)
        {
            ApplicationUser user = await db.Users.FindAsync(userId);
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("AdminPanel");
        }

        public IActionResult MyReviews()
        {
            MyViewModel mvm = new MyViewModel();

            var userId = db.Users.First(u => u.UserName == User.Identity.Name).Id;
            var reviewsWithLocation = db.Reviews
                .Where(rev => rev.UserId == userId)
                .Include(r => r.Location)
                .ToList();

            var myReviewsWithLocation = reviewsWithLocation.GroupBy(rev => rev.UserId).ToList();

            mvm.myReviewsWithLocation = myReviewsWithLocation;
            mvm.locations = db.Locations;

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