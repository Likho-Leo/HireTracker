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

       
    }
}
