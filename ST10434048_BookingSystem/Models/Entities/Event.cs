using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10434048_BookingSystem.Models.Entities
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        [Required(ErrorMessage = "Event must have a name")]
        [StringLength(100)]
        [Display(Name = "Event Name")]
        public required string EventName { get; set; }

        [Required(ErrorMessage = "Event must have a date associated with it")]
        [Display(Name = "Event Date")]
        public required DateOnly EventDate { get; set; }

        [Required(ErrorMessage = "Event must have a description")]
        [StringLength(500)]
        public required string Description { get; set; }

        // Foreign key for EventType
        [Required(ErrorMessage = "Event must have a type")]
        [Display(Name = "Event Type")]
        public int EventTypeID { get; set; }

        // Navigation property for EventType
        [ForeignKey("EventTypeID")]
        public virtual EventType? EventType { get; set; }

        // Navigation property for bookings
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}