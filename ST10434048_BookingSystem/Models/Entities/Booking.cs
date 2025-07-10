using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10434048_BookingSystem.Models.Entities
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        public required Guid VenueId { get; set; }

        [Required]
        public required Guid EventId { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        public required DateOnly BookingDate { get; set; }

        // Foreign key relationships
        [ForeignKey("VenueId")]
        public virtual Venue? Venue { get; set; }

        [ForeignKey("EventId")]
        public virtual Event? Event { get; set; }
    }
}