namespace ST10434048_BookingSystem.Models.View_Models
{
    public class BookingDetailsViewmodel
    {
        public Guid BookingId { get; set; }
        public DateOnly BookingDate { get; set; }
        public string EventName { get; set; }
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }
        public int VenueCapacity { get; set; }
        public DateOnly EventDate { get; set; }
        public int EventTypeID { get; set; }

        public string EventTypeName { get; set; }
    }
}
