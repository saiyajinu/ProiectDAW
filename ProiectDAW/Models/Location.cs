using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    public class Location
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;       
        public string Schedule { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
