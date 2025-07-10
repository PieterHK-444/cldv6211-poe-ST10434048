using System.ComponentModel.DataAnnotations;
using ST10434048_BookingSystem.Models.Entities;

namespace ST10434048_BookingSystem.Models.View_Models
{
    public class SearchViewModel
    {
        [Display(Name = "Booking ID")]
        public Guid? BookingId { get; set; }

        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        [Display(Name = "Event Type")]
        public int? EventTypeID { get; set; }

        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly? StartDate { get; set; }

        [Display(Name = "EndDate")]
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }
        public bool? VenueAvailable {get; set;}

        public bool HasSearched { get; set; }

        public List<BookingDetailsViewmodel> Bookings { get; set; } = new List<BookingDetailsViewmodel>();

        // Navigation property for EventType (for dropdown display)
        public EventType? EventType { get; set; }
    }
}