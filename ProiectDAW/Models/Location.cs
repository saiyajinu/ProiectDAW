namespace ProiectDAW.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; }
    }
}
