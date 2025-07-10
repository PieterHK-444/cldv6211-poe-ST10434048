using ST10434048_BookingSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

public class EventType
{
    [Key]
    public int EventTypeID { get; set; }  // Match the database column name

    [Required]
    [StringLength(50)]
    public string EventTypeName { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation property
    public virtual ICollection<Event>? Events { get; set; }
}