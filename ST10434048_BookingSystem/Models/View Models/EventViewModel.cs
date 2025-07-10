using ST10434048_BookingSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ST10434048_BookingSystem.Models.View_Models
{
    public class EventViewModel
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public DateOnly EventDate { get; set; }
        public string Description { get; set; }

        [Display(Name = "Event Type")]
        [Required(ErrorMessage = "Please select an event type")]
        public int EventTypeID { get; set; }

        // Navigation property for display
        public EventType? EventType { get; set; }
    }
}
