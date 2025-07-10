using Microsoft.AspNetCore.Http;
using ST10434048_BookingSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ST10434048_BookingSystem.Models.View_Models
{
    public class VenueViewModel
    {
        public Guid VenueId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Venue Name")]
        public required string VenueName { get; set; }

        [Required]
        [StringLength(255)]
        public required string Location { get; set; }

        [Required]
        [Range(1, 100000)]
        public required int Capacity { get; set; }

        // ImageUrl is optional here because it will be set after upload
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        // New property to hold the uploaded image file
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
