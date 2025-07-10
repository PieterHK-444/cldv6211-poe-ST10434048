using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ST10434048_BookingSystem.Models.Entities
{
    public class Venue
    {
        [Key]
        public Guid VenueId { get; set; }
        
        [Required(ErrorMessage ="Venue Must have a name")]
        [StringLength(100)]
        [Display(Name = "Venue Name")]
        public required string VenueName { get; set; }
        
        [Required(ErrorMessage ="Venue must have a location")]
        [StringLength(255)]
        public required string Location { get; set; }
        
        [Required(ErrorMessage = "Venue must have a capacity between 1 adn 100000")]
        [Range(1, 100000)]
        public required int Capacity { get; set; }
        
        [Required(ErrorMessage = "Venue must have an Image associated with it")]
        [Display(Name = "Image")]
        public required string ImageUrl { get; set; }
        
        // Navigation property - needed for Entity Framework relationships
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}