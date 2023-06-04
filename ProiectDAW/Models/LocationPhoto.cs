using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class LocationPhoto
    {
        [Key]
        public int PhotoId { get; set; }
        public string LocationId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
