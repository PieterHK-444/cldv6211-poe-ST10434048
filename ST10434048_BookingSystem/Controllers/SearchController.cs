using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10434048_BookingSystem.Data;
using ST10434048_BookingSystem.Models.View_Models;

namespace ST10434048_BookingSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookingsDbContext _context;

        public SearchController(BookingsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await PopulateDropdowns();
            return View(new SearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchViewModel model)
        {
            model.HasSearched = true;

            // Build query from the actual database tables with proper joins
            var query = _context.Bookings
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .AsQueryable();

            // Apply filters based on search criteria
            if (model.BookingId.HasValue && model.BookingId != Guid.Empty)
            {
                query = query.Where(b => b.BookingId == model.BookingId.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.EventName))
            {
                query = query.Where(b => b.Event.EventName.Contains(model.EventName));
            }

            if (model.EventTypeID.HasValue)
            {
                query = query.Where(b => b.Event.EventTypeID == model.EventTypeID.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.VenueName))
            {
                query = query.Where(b => b.Venue.VenueName.Contains(model.VenueName));
            }


            if (model.StartDate.HasValue)
            {
                query = query.Where(b => b.Event.EventDate >= model.StartDate.Value);
            }

            if (model.EndDate.HasValue)
            {
                query = query.Where(b => b.Event.EventDate <= model.EndDate.Value);
            }

           

            // Execute query and map to BookingDetailsViewmodel
            var bookings = await query
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            model.Bookings = bookings.Select(b => new BookingDetailsViewmodel
            {
                BookingId = b.BookingId,
                EventName = b.Event.EventName,
                EventTypeName = b.Event.EventType?.EventTypeName ?? "Unknown",
                EventTypeID = b.Event.EventTypeID,
                VenueName = b.Venue.VenueName,
                EventDate = b.Event.EventDate,
                BookingDate = b.BookingDate,
                VenueLocation = b.Venue.Location,
                VenueCapacity = b.Venue.Capacity,
            }).ToList();

            // Repopulate dropdowns for the view
            await PopulateDropdowns();

            return View(model);
        }

        /// <summary>
        /// Populates dropdown lists for the search form
        /// </summary>
        private async Task PopulateDropdowns()
        {
            // EventTypes dropdown
            var eventTypes = await _context.EventTypes
                .Where(et => et.IsActive)
                .OrderBy(et => et.EventTypeName)
                .Select(et => new SelectListItem
                {
                    Value = et.EventTypeID.ToString(),
                    Text = et.EventTypeName
                })
                .ToListAsync();

            eventTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- All Event Types --"
            });

            ViewBag.EventTypes = eventTypes;

            // You can add more dropdowns here if needed
            // For example, Venues dropdown:
            var venues = await _context.Venues
                .OrderBy(v => v.VenueName)
                .Select(v => new SelectListItem
                {
                    Value = v.VenueName,
                    Text = $"{v.VenueName} - {v.Location}"
                })
                .ToListAsync();

            venues.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- All Venues --"
            });

            ViewBag.Venues = venues;
        }
    }
}