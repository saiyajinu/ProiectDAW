using Microsoft.EntityFrameworkCore;

namespace ProiectDAW.Models
{
    public class MyViewModel
    {
        public IEnumerable<Review>? reviewsIEn;
        public IEnumerable<Location>? locationsIEn;
        public IEnumerable<ApplicationUser>? usersIEn;
        public DbSet<ApplicationUser>? users;
        public DbSet<Location>? locations;
        public DbSet<Review>? reviewsDb;
        public List<IGrouping<string, Review>> myReviewsWithLocation;

        public MyViewModel() 
        { 
        }
      
    }
}
