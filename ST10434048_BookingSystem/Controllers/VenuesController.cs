using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10434048_BookingSystem.Data;
using ST10434048_BookingSystem.Models.Entities;
using ST10434048_BookingSystem.Models.View_Models;

namespace ST10434048_BookingSystem.Controllers
{
    public class VenuesController : Controller
    {
        private readonly BookingsDbContext _context;

        public VenuesController(BookingsDbContext context)
        {
            _context = context;
        }

        public IActionResult Add()
        {
            return View(new VenueViewModel
            {
                Capacity = 0,
                VenueName = string.Empty,
                Location = string.Empty,
                ImageUrl = string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(VenueViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            bool venueExists = await _context.Venues
                .AnyAsync(v => v.VenueName == viewmodel.VenueName && v.Location == viewmodel.Location);

            if (venueExists)
            {
                ModelState.AddModelError(string.Empty, "A venue with the same name and location already exists.");
                return View(viewmodel);
            }

            string imageUrl = null;

            // Handle file upload to local storage instead of blob storage
            if (viewmodel.ImageFile != null && viewmodel.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(viewmodel.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF) are allowed.");
                    return View(viewmodel);
                }

                if (viewmodel.ImageFile.Length > 5 * 1024 * 1024) // 5MB max
                {
                    ModelState.AddModelError("ImageFile", "File size cannot exceed 5MB.");
                    return View(viewmodel);
                }

                // Save file to wwwroot/images/venues/
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "venues");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewmodel.ImageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/images/venues/{fileName}";
            }

            var venueentity = new Venue
            {
                VenueId = viewmodel.VenueId != Guid.Empty ? viewmodel.VenueId : Guid.NewGuid(),
                VenueName = viewmodel.VenueName,
                Location = viewmodel.Location,
                Capacity = viewmodel.Capacity,
                ImageUrl = imageUrl ?? viewmodel.ImageUrl
            };

            try
            {
                await _context.Venues.AddAsync(venueentity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the venue. Please try again.");
                return View(viewmodel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DisplayVenues()
        {
            var venues = _context.Venues.ToList();
            return View(venues);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            var viewmodel = new VenueViewModel
            {
                VenueId = venue.VenueId,
                VenueName = venue.VenueName,
                Location = venue.Location,
                Capacity = venue.Capacity,
                ImageUrl = venue.ImageUrl
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VenueViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            var venue = await _context.Venues.FindAsync(viewmodel.VenueId);
            if (venue == null)
            {
                return NotFound();
            }

            try
            {
                // Handle image upload
                if (viewmodel.ImageFile != null && viewmodel.ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(viewmodel.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Only image files (JPG, PNG, GIF) are allowed.");
                        return View(viewmodel);
                    }

                    if (viewmodel.ImageFile.Length > 5 * 1024 * 1024) // 5MB max
                    {
                        ModelState.AddModelError("ImageFile", "File size cannot exceed 5MB.");
                        return View(viewmodel);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(venue.ImageUrl) && venue.ImageUrl.StartsWith("/images/venues/"))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", venue.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Save new image
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "venues");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewmodel.ImageFile.CopyToAsync(fileStream);
                    }

                    venue.ImageUrl = $"/images/venues/{fileName}";
                }
                else
                {
                    // Preserve existing image if no new upload
                    venue.ImageUrl = viewmodel.ImageUrl;
                }

                // Update other fields
                venue.VenueName = viewmodel.VenueName;
                venue.Location = viewmodel.Location;
                venue.Capacity = viewmodel.Capacity;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                return View(viewmodel);
            }

            return RedirectToAction("DisplayVenues", "Venues");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var venueToDelete = await _context.Venues.FindAsync(id);
            if (venueToDelete == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            if (hasBookings)
            {
                ViewData["ErrorMessage"] = "Cannot delete Venue because there are existing bookings linked to it." +
                                           "<a href='/Bookings/DisplayBookings'>View Bookings</a>" + " or " + "<a href='/Search'>Search</a>";

                var viewmodel = new VenueViewModel
                {
                    VenueId = venueToDelete.VenueId,
                    VenueName = venueToDelete.VenueName,
                    Location = venueToDelete.Location,
                    Capacity = venueToDelete.Capacity,
                    ImageUrl = venueToDelete.ImageUrl
                };

                return View("Edit", viewmodel);
            }

            // Delete associated image file if it exists
            if (!string.IsNullOrEmpty(venueToDelete.ImageUrl) && venueToDelete.ImageUrl.StartsWith("/images/venues/"))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", venueToDelete.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Venues.Remove(venueToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction("DisplayVenues", "Venues");
        }
    }
}