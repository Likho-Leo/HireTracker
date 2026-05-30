using System.Diagnostics;
using HireTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.Where(b => !b.IsDeleted).ToListAsync();

            ViewBag.TotalBookings = bookings.Count;
            ViewBag.PendingTrips = bookings.Count(b => b.TripStatus == "Pending");
            ViewBag.CompletedTrips = bookings.Count(b => b.TripStatus == "Completed");
            ViewBag.OutstandingPayments = bookings.Where(b => b.PaymentStatus == "Owing" || b.PaymentStatus == "Partial").Sum(b => b.AmountDue - b.AmountPaid);

            var recentBookings = bookings.OrderByDescending(b => b.CreatedAt).Take(5).ToList();

            return View(recentBookings);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
