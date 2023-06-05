using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProiectDAW.Data;
using ProiectDAW.Models;
using System.Data;

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

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index(int locationId)
        {

            IEnumerable<Review> reviews = await db.Reviews.Where(r => r.LocationId == locationId).ToListAsync();
            Models.Location location = db.Locations.First(loc => loc.Id == locationId);
            ViewData["location"] = location.Name;
            ViewBag.locationId = locationId;
            MyViewModel myViewModel = new MyViewModel();
            myViewModel.users = db.Users;
            myViewModel.reviews = reviews;
            return View(myViewModel);
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Create(int locationId)
        {
            ViewBag.locationId = locationId;
            Models.Location location = db.Locations.First(loc => loc.Id == locationId);
            ViewData["location"] = location.Name;
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            ApplicationUser user = db.Users.First(u => u.UserName == User.Identity.Name);
            review.UserId = user.Id;
            if (ModelState.IsValid)
            {
                db.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index", new { locationId = review.LocationId });
            }
            return View(review);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Edit(int? reviewId)
        {
            if (reviewId == null || reviewId == 0)
            {
                return NotFound();
            }
            var reviewFromDb = await db.Reviews.FirstOrDefaultAsync(rev => rev.Id == reviewId);
            if (reviewFromDb == null)
            {
                return NotFound();
            }
            ViewData["reviewId"] = reviewId;
            var locationId = db.Reviews.Find(reviewId).LocationId;
            ViewBag.locationId = locationId;
            ViewData["location"] = db.Locations.Find(locationId).Name;
            return View(reviewFromDb);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int reviewId, Review review)
        {
            var id = reviewId;
            review.User = db.Users.First(u => u.UserName == User.Identity.Name);
            review.Id = id;
            if(ModelState.IsValid)
            {
                db.Update(review);
                db.SaveChanges();

                return RedirectToAction("Index", new { locationId = review.LocationId });
            }
            return View(review);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int? reviewId)
        {
            if (reviewId == null || reviewId == 0)
            {
                return NotFound();
            }
            var reviewFromDb = await db.Reviews.FirstOrDefaultAsync(rev => rev.Id == reviewId);
            if (reviewFromDb == null)
            {
                return NotFound();
            }
            ViewData["reviewId"] = reviewId;
            var locationId = db.Reviews.Find(reviewId).LocationId;
            ViewBag.locationId = locationId;
            ViewData["location"] = db.Locations.Find(locationId).Name;
            return View(reviewFromDb);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int reviewId)
        {
            var review = db.Reviews.Find(reviewId);
            if(review == null)
            {
                return NotFound();
            }
            db.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index", new { locationId = review.LocationId });


        }
    }
}

