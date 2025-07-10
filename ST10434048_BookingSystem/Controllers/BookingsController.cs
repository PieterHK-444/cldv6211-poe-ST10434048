using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10434048_BookingSystem.Data;
using ST10434048_BookingSystem.Models;
using ST10434048_BookingSystem.Models.Entities;

namespace ST10434048_BookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly BookingsDbContext _context;

        public BookingsController(BookingsDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Add()
        {
            var viewModel = new AddBookingViewModel
            {
                BookingId = Guid.NewGuid(),
                Venues = await _context.Venues
                    .Select(v => new SelectListItem
                    {
                        Value = v.VenueId.ToString(),
                        Text = $"{v.VenueName} - {v.Location}"
                    }).ToListAsync(),

                Events = await _context.Events
                    .Select(e => new SelectListItem
                    {
                        Value = e.EventId.ToString(),
                        Text = $"{e.EventName} - {e.EventDate.ToShortDateString()}"
                    }).ToListAsync(),

                BookingDate = DateOnly.FromDateTime(DateTime.Today)
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddBookingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns before returning view
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            var existingBooking = await _context.Bookings
                .AnyAsync(b => b.VenueId == viewModel.VenueId
                            && b.EventId == viewModel.EventId
                            && b.BookingDate == viewModel.BookingDate);

            if (existingBooking)
            {
                ModelState.AddModelError(string.Empty, "A booking already exists for this venue and event on the selected date.");
                await PopulateDropdowns(viewModel);
                return View(viewModel);
            }

            var booking = new Booking
            {
                BookingId = viewModel.BookingId,
                VenueId = viewModel.VenueId,
                EventId = viewModel.EventId,
                BookingDate = viewModel.BookingDate
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("DisplayBookings", "Bookings");
        }

        // Helper method to populate dropdowns
        private async Task PopulateDropdowns(AddBookingViewModel viewModel)
        {
            viewModel.Venues = await _context.Venues
                .Select(v => new SelectListItem
                {
                    Value = v.VenueId.ToString(),
                    Text = $"{v.VenueName} - {v.Location}"
                }).ToListAsync();

            viewModel.Events = await _context.Events
                .Select(e => new SelectListItem
                {
                    Value = e.EventId.ToString(),
                    Text = $"{e.EventName} - {e.EventDate.ToShortDateString()}"
                }).ToListAsync();
        }


        [HttpGet]
        public async Task<IActionResult> DisplayBookings()
        {
            // Retrieve the list of bookings from the database
            var bookings = await _context.Bookings.
                Include(b => b.Venue).Include(b => b.Event).ToListAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> EditBooking(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return View(null);
            }
            var viewModel = new AddBookingViewModel
            {
                BookingId = booking.BookingId,
                VenueId = booking.VenueId,
                EventId = booking.EventId,
                BookingDate = booking.BookingDate,
                Venues = await _context.Venues
                    .Select(v => new SelectListItem
                    {
                        Value = v.VenueId.ToString(),
                        Text = $"{v.VenueName} - {v.Location}"
                    }).ToListAsync(),
                Events = await _context.Events
                    .Select(e => new SelectListItem
                    {
                        Value = e.EventId.ToString(),
                        Text = $"{e.EventName} - {e.EventDate.ToShortDateString()}"
                    }).ToListAsync()
            };
            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(AddBookingViewModel viewmodel)
        {

            if (!ModelState.IsValid)
            {
                viewmodel.Venues = await _context.Venues
                     .Select(v => new SelectListItem
                     {
                         Value = v.VenueId.ToString(),
                         Text = $"{v.VenueName} - {v.Location}"
                     }).ToListAsync();
                viewmodel.Events = await _context.Events
                    .Select(e => new SelectListItem
                    {
                        Value = e.EventId.ToString(),
                        Text = $"{e.EventName} - {e.EventDate.ToShortDateString()}"
                    }).ToListAsync();
                return View(viewmodel);
            }
            var booking = await _context.Bookings.FindAsync(viewmodel.BookingId);
            if (booking is not null)
            {
                booking.VenueId = viewmodel.VenueId;
                booking.EventId = viewmodel.EventId;
                booking.BookingDate = viewmodel.BookingDate;
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DisplayBookings", "Bookings");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBooking(Booking booking)
        {
            var bookingToDelete = await _context.Bookings.FindAsync(booking.BookingId);
            if (bookingToDelete != null)
            {
                _context.Bookings.Remove(bookingToDelete);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DisplayBookings", "Bookings");
        }
    }
}
