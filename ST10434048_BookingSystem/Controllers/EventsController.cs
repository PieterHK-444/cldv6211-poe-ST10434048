using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10434048_BookingSystem.Data;
using ST10434048_BookingSystem.Models.Entities;
using ST10434048_BookingSystem.Models.View_Models;

namespace ST10434048_BookingSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly BookingsDbContext _context;

        public EventsController(BookingsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var viewModel = new EventViewModel();
            await PopulateEventTypesDropdown();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EventViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateEventTypesDropdown();
                return View(viewmodel);
            }

            // Check if an event with the same name and date already exists
            var existingEvent = await _context.Events
                .AnyAsync(e => e.EventName == viewmodel.EventName && e.EventDate == viewmodel.EventDate);

            if (existingEvent)
            {
                ModelState.AddModelError(string.Empty, "An event with the same name and date already exists.");
                await PopulateEventTypesDropdown();
                return View(viewmodel);
            }

            var eventEntity = new Event
            {
                EventId = viewmodel.EventId != Guid.Empty ? viewmodel.EventId : Guid.NewGuid(),
                EventName = viewmodel.EventName,
                EventDate = viewmodel.EventDate,
                Description = viewmodel.Description,
                EventTypeID = viewmodel.EventTypeID
            };

            try
            {
                await _context.Events.AddAsync(eventEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("DisplayEvents", "Events");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the event. Please try again.");
                await PopulateEventTypesDropdown();
                return View(viewmodel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DisplayEvents()
        {
            var events = await _context.Events
                .Include(e => e.EventType)
                .ToListAsync();
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var eventEntity = await _context.Events.FindAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var viewModel = new EventViewModel
            {
                EventId = eventEntity.EventId,
                EventName = eventEntity.EventName,
                EventDate = eventEntity.EventDate,
                Description = eventEntity.Description,
                EventTypeID = eventEntity.EventTypeID
            };

            await PopulateEventTypesDropdown(eventEntity.EventTypeID);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateEventTypesDropdown(viewModel.EventTypeID);
                return View(viewModel);
            }

            var eventEntity = await _context.Events.FindAsync(viewModel.EventId);
            if (eventEntity == null)
            {
                return NotFound();
            }

            try
            {
                eventEntity.EventName = viewModel.EventName;
                eventEntity.EventDate = viewModel.EventDate;
                eventEntity.Description = viewModel.Description;
                eventEntity.EventTypeID = viewModel.EventTypeID;

                await _context.SaveChangesAsync();
                return RedirectToAction("DisplayEvents", "Events");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the event. Please try again.");
                await PopulateEventTypesDropdown(viewModel.EventTypeID);
                return View(viewModel);
            }
        }

        [HttpPost]
        [Route("Events/Delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventToDelete == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);
            if (hasBookings)
            {
                ViewData["ErrorMessage"] = "Cannot delete event because there are existing bookings linked to it." +
                                         "<a href='/Bookings/DisplayBookings'>View Bookings</a>" + " or " + "<a href='/Search'>Search</a>";

                // Convert to ViewModel for the Edit view
                var viewModel = new EventViewModel
                {
                    EventId = eventToDelete.EventId,
                    EventName = eventToDelete.EventName,
                    EventDate = eventToDelete.EventDate,
                    Description = eventToDelete.Description,
                    EventTypeID = eventToDelete.EventTypeID
                };

                await PopulateEventTypesDropdown(eventToDelete.EventTypeID);
                return View("Edit", viewModel);
            }

            try
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("DisplayEvents", "Events");
            }
            catch (Exception ex)
            {
                // Handle any deletion errors
                ViewData["ErrorMessage"] = "An error occurred while deleting the event. Please try again.";
                var viewModel = new EventViewModel
                {
                    EventId = eventToDelete.EventId,
                    EventName = eventToDelete.EventName,
                    EventDate = eventToDelete.EventDate,
                    Description = eventToDelete.Description,
                    EventTypeID = eventToDelete.EventTypeID
                };

                await PopulateEventTypesDropdown(eventToDelete.EventTypeID);
                return View("Edit", viewModel);
            }
        }

        /// <summary>
        /// Populates the EventTypes dropdown for the views
        /// </summary>
        /// <param name="selectedEventTypeId">The currently selected EventType ID</param>
        private async Task PopulateEventTypesDropdown(int? selectedEventTypeId = null)
        {
            var eventTypes = await _context.EventTypes
                .Where(et => et.IsActive)
                .OrderBy(et => et.EventTypeName)
                .Select(et => new SelectListItem
                {
                    Value = et.EventTypeID.ToString(),
                    Text = et.EventTypeName,
                    Selected = selectedEventTypeId.HasValue && et.EventTypeID == selectedEventTypeId.Value
                })
                .ToListAsync();

            // Add a default "Select Event Type" option
            eventTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select Event Type --",
                Selected = !selectedEventTypeId.HasValue
            });

            ViewBag.EventTypes = eventTypes;
        }
    }
}