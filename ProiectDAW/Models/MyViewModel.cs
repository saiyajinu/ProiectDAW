using Microsoft.EntityFrameworkCore;

namespace ProiectDAW.Models
{
    public class MyViewModel
    {
        public IEnumerable<Review> reviews;
        public DbSet<ApplicationUser> users;
        public DbSet<Location> locations;
        public DbSet<Review> reviewsDb;

        public MyViewModel() 
        { 
        }
      
    }
}
