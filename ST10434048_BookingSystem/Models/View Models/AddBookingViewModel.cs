using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ST10434048_BookingSystem.Models
{
    public class AddBookingViewModel
    {
        public Guid BookingId { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public Guid VenueId { get; set; }

        [Required]
        [Display(Name = "Event")]
        public Guid EventId { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        public DateOnly BookingDate { get; set; }

        // Dropdown lists
        public List<SelectListItem>? Venues { get; set; }
        public List<SelectListItem>? Events { get; set; }
    }
}
