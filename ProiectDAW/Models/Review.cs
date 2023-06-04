namespace ProiectDAW.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;  
        public virtual ApplicationUser User { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; } 
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
