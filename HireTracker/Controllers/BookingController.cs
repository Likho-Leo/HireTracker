using HireTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HireTracker.Controllers
{
    //Any unauthorized user trying to access any Booking page will be automatically redirected to the login page
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context) => _context = context;

        //ASP.NET automatically reads 'search', 'tripStaus', and 'paymentStatus' from the URL query strign.
        public async Task<IActionResult> Index(string search, string tripStatus, string paymentStatus, int? dateRange)
        {
            //This doesn't hit the db yet, just builds a query. EF waits until 'ToListAsync()' is called
            var bookings = _context.Bookings.Where(b => !b.IsDeleted);

            //add search filter
            if (!string.IsNullOrEmpty(search))
            {
                bookings = bookings.Where(b =>
                    b.CompanyName.Contains(search) ||
                    b.Pickup.Contains(search) ||
                    b.Dropoff.Contains(search));
            }

            //add trip status filter
            if (!string.IsNullOrEmpty(tripStatus) && tripStatus != "All")
            {
                bookings = bookings.Where(b => b.TripStatus == tripStatus);
            }

            //add payment status filter
            if (!string.IsNullOrEmpty(paymentStatus) && paymentStatus != "All")
            {
                bookings = bookings.Where(b => b.PaymentStatus == paymentStatus);
            }
            
            //add date range filter
            if (dateRange.HasValue)
            {
                var fromDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-dateRange.Value));
                bookings = bookings.Where(b => b.Date >= fromDate);
            }

            //store values in view bag.
            //this passes current filter back to view so the searchbox and dropdowns stay filled in after the page reloads
            ViewBag.Search = search;
            ViewBag.TripStatus = tripStatus;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.DateRange = dateRange;

            //execute query. Only now does EF hit the db, with all filter combined into one efficient SQL query.
            return View(await bookings.OrderByDescending(b => b.CreatedAt).ToListAsync());
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
            //checks all '[Required]' annotations pass before saving-
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

        //Retrive selected record to be updated
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return BadRequest();       //security check to make sure ID in the URL matches the booking being edited

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingId == id && !m.IsDeleted);        //Fetches booking by id
            if (booking == null) return NotFound();     //returns a 404 if the booking doesn't exist'
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    booking.AmountDue = booking.Passengers * booking.PricePerhead;      //recalculate incase passangers or price per head changes
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["msg"] = "Booking successfully updated!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    TempData["msg"] = "Booking could not be updated. Please try again.";
                }
            }

            return View(booking);
        }

        [HttpGet]
        //Shows confirmation page
        public async Task<IActionResult> Delete (int? id)
        {
            if (id == null || id == 0) return NotFound();

            var booking = await _context.Bookings
                .AsNoTracking()     //tells EF not to track changes to the booking object since we're just displaying and not editing it.
                .FirstOrDefaultAsync(m => m.BookingId == id && !m.IsDeleted);       //

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]        //Two methods can't have the same name so this is renamed to 'DelteConfirmed' but tell ASP.NET to treat it as the 'Delete' action
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null) return NotFound();   
                
                booking.IsDeleted = true;
                _context.Update(booking);               //Change isDeleted true for soft delete
                await _context.SaveChangesAsync();      //commits the deletion to the database
                TempData["msg"] = "Booking successfully deleted!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["msg"] = "Booking could not be deleted. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //Shows all the details of a single booking
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var booking = await _context.Bookings
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }
    }
}
