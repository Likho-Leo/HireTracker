using HireTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HireTracker.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context) => _context = context;

        //Returns booking records from the db
        //async/await methods for db calls so app doesn't freeze while waiting for data
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bookings.ToListAsync());
        }

        //Redirects to the create booking/ new booking view
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]      //This only fires when form is submitted
        [ValidateAntiForgeryToken]      //Prevents cross site request fogery attacks
        public async Task<IActionResult> Create(Booking booking)
        {
            //checks all '[Required]' annotations pass before saving
            if (ModelState.IsValid)
            {
                try
                {
                    booking.CreatedAt = DateTime.Now;
                    booking.AmountDue = booking.PricePerhead * booking.Passengers;
                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["msg"] = "Booking successfully added!";
                    return RedirectToAction(nameof(Index));     
                }
                catch (DbUpdateException)
                {
                    TempData["msg"] = "Booking could not be saved. Please try again.";
                }
            }
            return View(booking);
        }

       
    }
}
